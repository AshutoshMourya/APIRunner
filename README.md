# ⚡ APIRunner v1.1.0
### The Fast, Lightweight API Runner That Actually Feels Good to Use.

[![.NET](https://img.shields.io/badge/.NET-5.0%2B-blue.svg)](https://dotnet.microsoft.com/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Release](https://img.shields.io/github/v/release/AshutoshMourya/APIRunner)](https://github.com/AshutoshMourya/APIRunner/releases)

**Tired of Postman’s sluggish Collection Runner?**  
The constant spinning wheels, multi-minute startup times, 800MB+ of RAM bloat, and that horrible lag when running even a medium-sized collection? I was *extremely* frustrated.

Postman has become a bloated Electron monster. **APIRunner** is the solution: a clean, blazing-fast C# console application designed to be the simple, high-performance Postman runner alternative you've been looking for.

---

## 🚀 Why APIRunner?

*   ⚡ **Instant Start**: No splash screens. No loading bars. Just execution.
*   🪶 **Ultra-Lightweight**: Minimal memory footprint (goodbye 1GB+ RAM bloat).
*   🖥️ **CLI First**: Pure terminal experience—perfect for local testing, automation, and CI/CD.
*   ✅ **Upcoming**: Full support for Postman-like collections, environments, variables, and pre-request scripts.

---

## ✨ Features

-   **Batch Processing**: Automatically reads bulk data from `data.csv`, handling headers seamlessly.
-   **Dynamic JSON Generation**: Maps CSV columns (`TransactionID`, `EmailAddress`, etc.) into structured JSON payloads on the fly.
-   **REST API Integration**: High-performance asynchronous POST requests using `HttpClient`.
-   **Rate Limiting**: Configurable throttling (default: 1s) to respect API rate limits.
-   **Detailed Logging**: Real-time console output + persistent logging to `response.txt`.
-   **Robust Error Handling**: Processes entire batches without stopping for individual row failures.

---

## 🆕 New in v1.2.0
-   **Robust CSV Parsing**: Integrated `CsvHelper` to safely handle edge cases like internal commas, quoted strings, and direct-to-object mapping (goodbye `String.Split`).

## 🆕 New in v1.1.0
-   **Configuration Management**: No more hardcoding! All settings now live in `appsettings.json`.
-   **Environment Overrides**: Support for overriding configuration via System Environment Variables.
-   **Repository Hygiene**: Cleaned up build artifacts with optimized `.gitignore`.

---

## 🛠️ Setup & Configuration

### 1. Prerequisites
-   [.NET SDK](https://dotnet.microsoft.com/download) (v5.0 or higher)
-   A `data.csv` file in the root directory.

### 2. Prepare your Data
Ensure your `data.csv` follows this format:
```csv
TransactionID,EmailAddress,CreationDate
TX_001,user1@example.com,2024-03-15
TX_002,user2@example.com,2024-03-15
```

### 3. Configure Settings
Edit `appsettings.json` to point to your API:
```json
{
  "ApiSettings": {
    "Url": "https://api.your-domain.com/v1/endpoint",
    "DelayMilliseconds": 1000
  },
  "FileSettings": {
    "CsvPath": "data.csv"
  }
}
```

---

## 🏃 Usage

Run the application instantly from your terminal:

```bash
dotnet run
```

### Expected Output:
```text
[2024-03-15 11:30:00] Sent: user1@example.com | Status: OK | Response: {"id": "123"}
Waiting 1000ms before next request...
[2024-03-15 11:30:01] Sent: user2@example.com | Status: Created | Response: {"id": "124"}
All requests processed.
```

---

## 💡 Troubleshooting & Keywords

If you're having trouble with your current tools and found this via:
-   *“Newman alternative that’s actually fast”*
-   *“Postman runner too slow”*
-   *“Lightweight Postman collection runner CLI”*

…welcome home. If a request fails (network timeout, API down), APIRunner logs the error with a timestamp to `response.txt` and continues to the next item—ensuring your batch jobs never hang.

---

## 🤝 Contribution
Found a bug or want a feature (like collection support)? Open an Issue or submit a PR!
