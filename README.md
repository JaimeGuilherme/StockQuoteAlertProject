# 📈 StockQuoteAlertProject

C# application that monitors stock prices on B3 in real-time (for each minute) and sends automated email alerts when assets reach predefined buy/sell thresholds. Perfect for investors seeking timely notifications on market opportunities.

---

## ⚙️ Essential Precondition

- [.NET SDK 9+](https://dotnet.microsoft.com/en-us/download)
- Email account with SMTP access (e.g., Gmail, Outlook, etc)
- Access token for the [Brapi API](https://brapi.dev/)

---

## 🔧 Arquivo de Configuração (`config.json`)

Para configurar o sistema, crie um arquivo chamado `config.json` na raiz do projeto com a seguinte estrutura:

```json
{
  "Email": {
    "Recipients": ["exemplo1@email.com", "exemplo2@email.com"]
  },
  "SMTP": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "User": "seu_email@provedor.com",
    "Password": "sua_senha_ou_senha_de_app",
    "Sender": "alerta@seudominio.com"
  },
  "Brapi": {
    "Token": "seu_token_de_acesso_brapi"
  },
  "MonitoringIntervalSeconds": 60
}
```

### Explicação detalhada dos campos:

| Campo                       | Tipo         | Descrição                                                                                 | Exemplo                                   |
|-----------------------------|--------------|-------------------------------------------------------------------------------------------|-------------------------------------------|
| **Email.Recipients**         | Array string | Lista de e-mails que receberão os alertas. Deve conter pelo menos um e-mail válido.      | `["email1@exemplo.com", "email2@exemplo.com"]` |
| **SMTP.Host**                | String       | Endereço do servidor SMTP do seu provedor de e-mail.                                     | `"smtp.gmail.com"`                         |
| **SMTP.Port**                | Inteiro      | Porta do servidor SMTP (número sem aspas).                                               | `587`                                     |
| **SMTP.User**                | String       | E-mail usado para autenticação no servidor SMTP.                                         | `"seu_email@provedor.com"`                 |
| **SMTP.Password**            | String       | Senha do e-mail ou senha de app (no caso de Gmail e outros).                             | `"sua_senha_ou_senha_de_app"`             |
| **SMTP.Sender**              | String       | Endereço de e-mail que aparecerá como remetente das mensagens enviadas.                  | `"alerta@seudominio.com"`                  |
| **Brapi.Token**              | String       | Token de acesso à API Brapi para autenticação nas requisições.                           | `"seu_token_de_acesso_brapi"`              |
| **MonitoringIntervalSeconds**| Inteiro      | Intervalo em segundos para checagem dos preços das ações. Deve ser um número inteiro.    | `60` (verifica a cada 60 segundos)        |

### Importante:

- Strings (texto) **devem estar entre aspas duplas `" "`**.
- Números inteiros **não devem ter aspas**.
- O campo `Recipients` aceita vários e-mails, separados por vírgulas e cada um entre aspas.
- A senha no campo `Password` deve ser mantida em segurança e **nunca** deve ser compartilhada publicamente.
- Para Gmail com autenticação em dois fatores, utilize uma **senha de app** em vez da sua senha normal.

---

## Exemplos de configuração SMTP

### Gmail

- **Host:** `smtp.gmail.com`
- **Porta:** `587` (TLS) ou `465` (SSL)
- **User:** seu e-mail Gmail completo (ex: `meuemail@gmail.com`)
- **Password:** sua senha de app (se usar autenticação em dois fatores) ou sua senha normal (não recomendado)
- **Sender:** e-mail remetente, geralmente igual ao usuário

**Dicas:**

- Se você usa autenticação em dois fatores no Gmail, **crie uma senha de app** para este sistema.
- Ative o acesso a apps menos seguros se não usar autenticação em dois fatores (não recomendado).
- Use porta 587 com TLS preferencialmente.

---

### Outlook / Hotmail / Microsoft 365

- **Host:** `smtp.office365.com`
- **Porta:** `587` (TLS)
- **User:** seu e-mail Outlook completo (ex: `meuemail@outlook.com`)
- **Password:** sua senha normal ou senha de app
- **Sender:** e-mail remetente, geralmente igual ao usuário

**Dicas:**

- Use porta 587 com TLS.
- Pode ser necessário configurar permissões na conta para permitir SMTP externo.
- Para contas corporativas, verifique com o administrador se o SMTP está liberado.

---

## Como rodar

Abra um terminal na pasta do projeto e execute o comando abaixo, passando o símbolo da ação, o preço de venda (limite superior) e o preço de compra (limite inferior) como argumentos:

```bash
dotnet run -- <STOCK_SYMBOL> <SELL_PRICE> <BUY_PRICE>
```

### Exemplo:

```bash
dotnet run -- PETR4 22.67 22.59
```

O programa irá monitorar continuamente o preço da ação e enviar alertas por e-mail sempre que o preço cruzar os limites definidos.

---

### 🛠️ Publicar e executar o projeto

Para compilar o projeto e gerar um executável `.exe`, siga os passos:

#### 1. Publicar o projeto

No terminal, dentro da pasta do projeto, rode:

```bash
dotnet publish -c Release -o publish
```

- Isso irá gerar os arquivos compilados na pasta `publish/`, incluindo o executável `stock-quote-alert.exe`.

#### 2. Executar o `.exe`

No terminal, navegue até a pasta `publish`:

```bash
cd publish
```

Agora execute o programa com os parâmetros desejados (exemplo com o ativo PETR4):

```bash
.\stock-quote-alert.exe PETR4 22.67 22.59
```

- `PETR4` é o símbolo da ação
- `22.67` é o preço alvo superior (limite para venda)
- `22.59` é o preço alvo inferior (limite para compra)