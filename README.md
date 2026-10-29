# DashboardTheKingThePower

Clone pixel-perfect do Apex Blazor Dashboard.

## Tech Stack

- Blazor WebAssembly (.NET 10)
- Tailwind CSS v4
- ApexCharts
- Phosphor Icons

## First Run

**Sem credenciais padrão.** Auth flow: register → login.

1. Acesse `/register`
2. Crie conta (nome, email, senha)
3. Faça login em `/`

**Dados**: localStorage apenas (limpar cache = perde tudo).

## Development

```bash
dotnet run --project src/DashboardTheKingThePower
```

## Build

```bash
dotnet publish src/DashboardTheKingThePower -c Release -o publish
```

## Deploy

**Demo**: https://dashboardthekingthepower.tiagords.com.br/