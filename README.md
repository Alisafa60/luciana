# Luciana Scarves

## Frontend
The frontend of this project is being built using React with Typescript.

## Backend
The backend is being developed with ASP.NET core.
Add `appsettings.local.json` file with:
```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Database_host;"
    },

    "JwtSettings": {
        "SecretKey": "Secret_Key"
    },

    "LuceneSettings": {
    "IndexDirectoryPath": "LuceneIndex"
  }
}
```

## Database
The database management system used in this project is PostgreSQL.

![ER Diagram](https://github.com/Alisafa60/luciana/blob/main/lucianaER.png)

