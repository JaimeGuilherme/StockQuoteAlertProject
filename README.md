# 📈 StockQuoteAlertProject
C# application that monitors stock prices on B3 in real-time and sends automated email alerts when assets reach predefined buy/sell thresholds. Perfect for investors seeking timely notifications on market opportunities.

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
    "Destino": "your@email.com"
  },
  "SMTP": {
    "Host": "smtp.seuprovedor.com",
    "Port": "587",
    "User": "your@email.com",
    "Password": "your_password/your_app_password(gmail)",
    "Sender": "alert@yourproject.com"
  },
  "Brapi": {
    "Token": "your_brapi_token"
  }
}
```

---

## How to run

Open a terminal in the project folder and run the command below, passing the stock symbol, sell price, and buy price as arguments:

```bash
dotnet run --project StockQuoteAlertProject.csproj <STOCK_SYMBOL> <SELL_PRICE> <BUY_PRICE>
```

### Example:

```bash
dotnet run --project StockQuoteAlertProject.csproj PETR4 22.67 22.59
```

The program will continuously monitor the stock price and send email alerts when the price crosses the defined thresholds.