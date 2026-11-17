# Secure local configuration

Do not commit `appsettings*.json`, credentials, or signing keys. The tracked `Backend/Api/appsettings.example.json` is a safe template only; copy it locally to `appsettings.Development.json` if needed. That local file is ignored by Git.

For local development, use .NET User Secrets from the API project directory, or supply environment variables. Use `ConnectionStrings:DefaultConnection`, `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`, and `Jwt:ExpireMinutes` as User Secrets keys. Environment-variable equivalents use double underscores, for example `ConnectionStrings__DefaultConnection` and `Jwt__Key`.

For deployment, inject these values through the deployment platform's secret store. Use a fresh, long random JWT signing key and rotate any credential that has ever been committed.
