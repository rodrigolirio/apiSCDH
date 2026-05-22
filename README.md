# SCDH — Sistema de Custódia Digital de Habitação

API em ASP.NET Core para registro de contratos imobiliários com persistência em SQLite, auditoria via middleware e política de CORS restritiva.

## Stack
- ASP.NET Core 10 (Web API)
- Entity Framework Core + SQLite
- Swagger (OpenAPI)

## Funcionalidades
- Upload de contrato (PDF até 5MB) com metadados (CPF, número, valor)
- Listagem de contratos persistidos
- Download do PDF por ID
- Middleware de auditoria registrando todas as requisições
- CORS restrito à origem `http://localhost:5100`, métodos GET e POST

## Como executar

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Acesse: `http://localhost:5167/swagger`

## Estrutura
- `Controllers/` — endpoints REST
- `Data/` — DbContext do EF Core
- `Models/` — entidade ContratoHabitacional
- `Middlewares/` — auditoria de requisições
- `wwwroot/documentos/` — armazenamento físico dos PDFs
- `habitacao.db` — banco SQLite

## Decisões técnicas
- **SQLite**: persistência local, ACID, zero configuração
- **Guid como PK**: gerado na aplicação, seguro para distribuição futura, não expõe sequência.
- **PDF no filesystem, path no banco**: backup mais simples, queries leves, padrão de separação metadado/blob.
- **GUID no nome do arquivo**: evita colisão e previne path traversal.
