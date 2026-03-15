using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

using var client = new HttpClient();
string url = config["ApiSettings:Url"] ?? "https://api-endpoint.com/api/v1/create";
int delayMilliseconds = int.TryParse(config["ApiSettings:DelayMilliseconds"], out int delay) ? delay : 1000;

// Standard Headers
client.DefaultRequestHeaders.Clear();
client.DefaultRequestHeaders.Add("USESERVICEBUSQUEUE", "false");
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

string csvPath = config["FileSettings:CsvPath"] ?? "data.csv";
var lines = File.ReadAllLines(csvPath);

// Iterating through the CSV
for (int index = 0; index < lines.Length - 1; index++)
{
    string line = lines[index + 1]; // Skip header
    var columns = line.Split(',');
    string TransactionID = columns[0];
    string emailAddress = columns[1];
    string creationDate = columns[2];

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

    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

    try 
    {
        var response = await client.PostAsync(url, content);
        string responseBody = await response.Content.ReadAsStringAsync();
        string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Sent: {emailAddress} | Status: {response.StatusCode} | Response: {responseBody}";
        
        Console.WriteLine(logLine);
        await File.AppendAllTextAsync("response.txt", logLine + Environment.NewLine);

        // --- THE DELAY LOGIC ---
        // Only delay if it's NOT the last item in the list
        if (index < lines.Length - 2) 
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