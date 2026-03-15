# API Runner

API Runner is a lightweight, asynchronous C# console application designed to read batch data from a CSV file, format it into structured JSON payloads, and send it sequentially to a REST API. It features built-in rate-limiting (delaying between requests) and comprehensive logging of responses and errors.

## Features

- **Batch Processing**: Reads bulk data from a `data.csv` file, skipping the header row automatically.
- **Automated JSON Payload Generation**: Maps CSV columns (`TransactionID`, `EmailAddress`, `CreationDate`) into a structured JSON request body.
- **REST API Integration**: Sends asynchronous POST requests using `HttpClient` to your configured API endpoint.
- **Rate Limiting / Throttling**: Configurable delay (default: 1 second) between consecutive API requests to prevent server throttling or overloading.
- **Detailed Logging**: Records execution logs containing timestamps, processed email addresses, HTTP status codes, and raw server response bodies. Logs are streamed to both the console and a local `response.txt` file.
- **Robust Error Handling**: Gracefully catches and logs exceptions without interrupting the entire batch process.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (Version 6.0 or higher recommended - uses top-level statements)
- A properly formatted `data.csv` file in the root directory of the application.

## Setup & Configuration

1. **Prepare your Data**:
   Ensure you have a `data.csv` file in the same directory where the executable runs. It expects the following format (comma-separated):
   ```csv
   TransactionID,EmailAddress,CreationDate
   TX123,user1@example.com,2023-10-25
   ...
   ```

2. **Configure the Script**:
   Open `Program.cs` and update the following configuration variables as needed:
   - `string url`: Replace with your actual target API endpoint URL.
   - `int delayMilliseconds`: Adjust the delay between requests (default is `1000` ms).
   - `string csvPath`: Update if your source file has a different name or path.
   - Additional custom request headers (e.g., an Authorization token) can be added to the `client.DefaultRequestHeaders`.

## Usage

Run the project directly using the .NET CLI from the project directory:

```bash
dotnet run
```

### Expected Output

During execution, the application will output its progress to the console and write it to `response.txt`:

```text
[2024-03-15 10:00:00] Sent: user1@example.com | Status: OK | Response: {"status":"success"}
Waiting 1000ms before next request...
[2024-03-15 10:00:01] Sent: user2@example.com | Status: Created | Response: {"status":"success"}
...
All requests processed.
```

## Error Handling

If a request fails (e.g., due to a network timeout, unreachable API, or invalid data), the exception is caught, formatted with a timestamp, and logged to both the console and `response.txt`. The program will then seamlessly proceed to process the next row in the CSV file.
