
# 📈 StockQuoteAlertProject

C# application that monitors B3 stock prices in real-time (every minute) and sends automated email alerts when assets reach predefined buy/sell thresholds. Perfect for investors seeking timely notifications about market opportunities.

---

## ⚙️ Essential Prerequisites

- [.NET SDK 9+](https://dotnet.microsoft.com/en-us/download)
- Email account with SMTP access (e.g., Gmail, Outlook, etc.)
- Access token for the [Brapi API](https://brapi.dev/)

---

## 🔧 Configuration File (`config.json`)

To configure the system, create a file named `config.json` in the project root with the following structure:

```json
{
  "Email": {
    "Recipients": ["example1@email.com", "example2@email.com"]
  },
  "SMTP": {
    "Host": "smtp.provider.com",
    "Port": 587,
    "User": "your_email@provider.com",
    "Password": "your_password_or_app_password",
    "Sender": "alert@yourdomain.com"
  },
  "Brapi": {
    "Token": "your_brapi_access_token"
  },
  "MonitoringIntervalSeconds": 60
}
```

### Detailed explanation of the fields:

| Field                     | Type         | Description                                                                             | Example                                  |
|---------------------------|--------------|-----------------------------------------------------------------------------------------|------------------------------------------|
| **Email.Recipients**       | Array string | List of emails that will receive alerts. Must contain at least one valid email address. | `["email1@example.com", "email2@example.com"]` |
| **SMTP.Host**              | String       | SMTP server address of your email provider.                                            | `"smtp.provider.com"`                     |
| **SMTP.Port**              | Integer      | SMTP server port (number without quotes).                                              | `587`                                    |
| **SMTP.User**              | String       | Email used for SMTP authentication.                                                    | `"your_email@provider.com"`               |
| **SMTP.Password**          | String       | Email password or app password (for Gmail and others).                                | `"your_password_or_app_password"`        |
| **SMTP.Sender**            | String       | Email address that will appear as the sender of the messages.                         | `"alert@yourdomain.com"`                  |
| **Brapi.Token**            | String       | Access token for the Brapi API to authenticate requests.                              | `"your_brapi_access_token"`               |
| **MonitoringIntervalSeconds**| Integer    | Interval in seconds to check stock prices. Must be an integer number.                  | `60` (checks every 60 seconds)            |

### Important:

- Do not forget any fields.
- Strings **must be enclosed in double quotes `" "`**.
- Integers **must NOT be enclosed in quotes**.
- The `Recipients` field accepts multiple emails, separated by commas, each inside quotes.
- The password in the `Password` field should be kept secure and **never shared publicly**.
- For Gmail with two-factor authentication, use an **app password** instead of your normal password.

---

## SMTP Configuration Examples

### Gmail

- **Host:** `smtp.gmail.com`
- **Port:** `587` (TLS) or `465` (SSL)
- **User:** your full Gmail email (e.g., `myemail@gmail.com`)
- **Password:** your app password (if using two-factor authentication) or your normal password (not recommended)
- **Sender:** sender email, usually the same as user

**Tips:**

- If you use two-factor authentication on Gmail, **create an app password** for this system.
- Enable access for less secure apps if not using two-factor authentication (not recommended).
- Prefer port 587 with TLS.

---

### Outlook / Hotmail / Microsoft 365

- **Host:** `smtp.office365.com`
- **Port:** `587` (TLS)
- **User:** your full Outlook email (e.g., `myemail@outlook.com`)
- **Password:** your normal password or app password
- **Sender:** sender email, usually the same as user

**Tips:**

- Use port 587 with TLS.
- You may need to configure permissions in your account to allow external SMTP.
- For corporate accounts, check with your administrator if SMTP is allowed.

---

## How to Run

Open a terminal in the project folder and run the command below, passing the stock symbol, sell price (upper limit), and buy price (lower limit) as arguments:

```bash
dotnet run -- <STOCK_SYMBOL> <SELL_PRICE> <BUY_PRICE>
```

### Example:

```bash
dotnet run -- PETR4 22.67 22.59
```

The program will continuously monitor the stock price and send email alerts whenever the price crosses the defined limits.

---

### 🛠️ Publish and Run the Project

To compile the project and generate an executable `.exe`, follow these steps:

#### 1. Publish the project

In the terminal inside the project folder, run:

```bash
dotnet publish -c Release -o publish
```

- This will generate the compiled files in the `publish/` folder, including the executable `stock-quote-alert.exe`.

#### 2. Run the `.exe`

In the terminal, navigate to the `publish` folder:

```bash
cd publish
```

Now run the program with the desired parameters (example with the stock PETR4):

```bash
.\stock-quote-alert.exe PETR4 22.67 22.59
```

- `PETR4` is the stock symbol
- `22.67` is the upper target price (sell limit)
- `22.59` is the lower target price (buy limit)
