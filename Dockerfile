FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/DashboardTheKingThePower/*.csproj src/DashboardTheKingThePower/
RUN dotnet restore src/DashboardTheKingThePower/*.csproj
COPY src/ src/
RUN dotnet publish src/DashboardTheKingThePower/*.csproj -c Release -o /app/publish --no-restore

FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY security-headers.conf /etc/nginx/security-headers.conf
EXPOSE 80
