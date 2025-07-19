# 📈 StockQuoteAlertProject
C# application that monitors stock prices on B3 in real-time (for each minute) and sends automated email alerts when assets reach predefined buy/sell thresholds. Perfect for investors seeking timely notifications on market opportunities.

---

## ⚙️ Essential Precondition

- [.NET SDK 9+](https://dotnet.microsoft.com/en-us/download)
- Email account with SMTP access (e.g., Gmail, Outlook, etc)
- Access token for the [Brapi API](https://brapi.dev/)

---

## 🔧 Settings

Create a file named `config.json` in the project root with the following content:

```json
{
  "Email": {
    "Destino": ["example1@email.com","example2@email.com"]
  },
  "SMTP": {
    "Host": "smtp.gmail.com",
    "Port": "587",
    "User": "your@email.com",
    "Password": "your_password or your_app_password(gmail)",
    "Sender": "alert@yourproject.com"
  },
  "Brapi": {
    "Token": "your_brapi_token"
  },
  "MonitoringIntervalSeconds": your time in seconds (int)
}
```

---

## How to run

Open a terminal in the project folder and run the command below, passing the stock symbol, sell price, and buy price as arguments:

```bash
dotnet run -- <STOCK_SYMBOL> <SELL_PRICE> <BUY_PRICE>
```

### Example:

```bash
dotnet run -- PETR4 22.67 22.59
```

The program will continuously monitor the stock price and send email alerts when the price crosses the defined thresholds.

---

### 🛠️ Publish and Run the Project

To compile the project and generate an executable `.exe`, follow these steps:

#### 1. Publish the project

In the terminal, inside the project folder, run:

```bash
dotnet publish -c Release -o publish
```

- This will generate the compiled files in the `publish/` folder, including the executable `stock-quote-alert.exe`.

#### 2. Run the `.exe`

In the terminal, navigate to the `publish` folder:

```bash
cd publish
```

Now, run the program with the desired parameters (example with the asset PETR4):

```bash
.\stock-quote-alert.exe PETR4 22.67 22.59
```

- `PETR4` is the ticker symbol
- `22.67` is the upper price target (sell threshold)
- `22.59` is the lower price target (buy threshold)