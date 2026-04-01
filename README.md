# ⚡ APIRunner v1.3.0
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

-   **Advanced Resilience**: Integrated Polly with exponential backoff to handle transient API failures, network timeouts, and specific HTTP error codes (e.g., 503, 429).
-   **Batch Processing**: Automatically reads bulk data from `data.csv`, handling headers seamlessly.
-   **Dynamic JSON Generation**: Maps CSV columns (`TransactionID`, `EmailAddress`, etc.) into structured JSON payloads on the fly.
-   **REST API Integration**: High-performance asynchronous POST requests using `HttpClient`.
-   **Rate Limiting**: Configurable throttling (default: 1s) to respect API rate limits.
-   **Detailed Logging**: Real-time console output + persistent logging to `response.txt`.
-   **Robust Error Handling**: Processes entire batches without stopping for individual row failures.

---

## 🆕 New in v1.3.0
-   **Advanced Resilience with Polly**: Added configurable retry policies with exponential backoff to handle transient API downtime, 503 errors, and network timeouts.

## 🆕 v1.2.0
-   **Robust CSV Parsing**: Integrated `CsvHelper` to safely handle edge cases like internal commas, quoted strings, and direct-to-object mapping (goodbye `String.Split`).

## 🆕 v1.1.0
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
  },
  "RetrySettings": {
    "MaxRetries": 3,
    "BaseDelaySeconds": 2
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

## 🤝 Contributing & Collaboration

**APIRunner** started as a personal tool to solve the bloat of modern API clients, but it thrives on community input. Whether you're a beginner or a veteran, your contributions are welcome!

### How to Help
-   **Reporting Bugs**: Found an edge case with a CSV or a weird API response? Open an [Issue](https://github.com/AshutoshMourya/APIRunner/issues).
-   **New Features**: Want to add support for JSON data sources, OAuth2 authentication, or a full CLI dashboard? We'd love to see it!
-   **Documentation**: Help us improve the README, add examples, or write a "Getting Started" guide.

### Contribution Workflow
1.  **Fork** the repository.
2.  **Clone** your fork (`git clone https://github.com/YOUR_USERNAME/APIRunner.git`).
3.  **Create a branch** for your feature/fix (`git checkout -b feature/cool-new-thing`).
4.  **Commit** your changes with descriptive messages.
5.  **Push** to your fork and **Open a Pull Request**.

### 🗺️ Future Roadmap (Ideas for Beginners)
- [ ] Support for `PUT` and `PATCH` methods.
- [ ] Support for custom HTTP headers via `appsettings.json`.
- [ ] Integration of a progress bar (e.g., using [Spectre.Console](https://spectreconsole.net)).
- [ ] Support for reading Postman Environment files.

---

## 📜 License

This project is licensed under the **MIT License**. Feel free to use, modify, and distribute it as you wish. See the [LICENSE](LICENSE) file for more details.
