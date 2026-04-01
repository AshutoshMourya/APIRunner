using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Linq;
using Polly;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

using var client = new HttpClient();
string url = config["ApiSettings:Url"] ?? "https://api-endpoint.com/api/v1/create";
int delayMilliseconds = int.TryParse(config["ApiSettings:DelayMilliseconds"], out int delay) ? delay : 1000;
int maxRetries = int.TryParse(config["RetrySettings:MaxRetries"], out int retries) ? retries : 3;
int baseDelaySeconds = int.TryParse(config["RetrySettings:BaseDelaySeconds"], out int baseDelay) ? baseDelay : 2;

// Standard Headers
client.DefaultRequestHeaders.Clear();
client.DefaultRequestHeaders.Add("USESERVICEBUSQUEUE", "false");
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

string csvPath = config["FileSettings:CsvPath"] ?? "data.csv";

using var reader = new StreamReader(csvPath);
using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
var records = csv.GetRecords<CsvRecord>().ToList();

var retryPolicy = Policy<HttpResponseMessage>
    .Handle<HttpRequestException>()
    .Or<TaskCanceledException>()
    .OrResult(r => r.StatusCode == System.Net.HttpStatusCode.RequestTimeout)        // 408
    .OrResult(r => r.StatusCode == System.Net.HttpStatusCode.TooManyRequests)       // 429
    .OrResult(r => r.StatusCode == System.Net.HttpStatusCode.InternalServerError)   // 500
    .OrResult(r => r.StatusCode == System.Net.HttpStatusCode.BadGateway)            // 502
    .OrResult(r => r.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)    // 503
    .OrResult(r => r.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)        // 504
    .WaitAndRetryAsync(
        retryCount: maxRetries,
        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(baseDelaySeconds, retryAttempt)),
        onRetry: (outcome, timespan, retryAttempt, context) =>
        {
            string reason = outcome.Exception?.Message 
                ?? $"HTTP {(int)outcome.Result.StatusCode} ({outcome.Result.StatusCode})";
            Console.WriteLine($"[Retry] Attempt {retryAttempt}/{maxRetries} — {reason}. Waiting {timespan.TotalSeconds}s...");
        });

// Iterating through the CSV
for (int index = 0; index < records.Count; index++)
{
    var record = records[index];
    string TransactionID = record.TransactionID;
    string emailAddress = record.EmailAddress;
    string creationDate = record.CreationDate;

    string jsonPayload = $@"{{
      ""Header"": {{
        ""TransactionID"":  ""{TransactionID}""
      }},
      ""ContentData"": {{
        ""Data"": [
          {{
            ""EmailAddress"": ""{emailAddress}"",
            ""CreationDate"": ""{creationDate}"",
            ""AddedBy"": ""Interface""
          }}
        ]
      }}
    }}";

    try 
    {
        var response = await retryPolicy.ExecuteAsync(async () => 
        {
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            return await client.PostAsync(url, content);
        });
        
        string responseBody = await response.Content.ReadAsStringAsync();
        string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Sent: {emailAddress} | Status: {response.StatusCode} | Response: {responseBody}";
        
        Console.WriteLine(logLine);
        await File.AppendAllTextAsync("response.txt", logLine + Environment.NewLine);

        // --- THE DELAY LOGIC ---
        // Only delay if it's NOT the last item in the list
        if (index < records.Count - 1) 
        {
            Console.WriteLine($"Waiting {delayMilliseconds}ms before next request...");
            await Task.Delay(delayMilliseconds);
        }
    }
    catch (Exception ex)
    {
        string errorLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Error processing {emailAddress}: {ex.Message}";
        Console.WriteLine(errorLine);
        await File.AppendAllTextAsync("response.txt", errorLine + Environment.NewLine);
    }
}

Console.WriteLine("All requests processed.");

public class CsvRecord
{
    [Index(0)]
    public string TransactionID { get; set; }
    [Index(1)]
    public string EmailAddress { get; set; }
    [Index(2)]
    public string CreationDate { get; set; }
}