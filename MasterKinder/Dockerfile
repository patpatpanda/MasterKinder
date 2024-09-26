# Byggfasen
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Kopiera csproj och återställ beroenden
COPY *.csproj ./
RUN dotnet restore

# Kopiera resten av applikationen och bygg
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Driftfasen
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Exponera API-porten (80 för HTTP, du kan välja vilken som helst för API:et)
EXPOSE 80

# Kör applikationen
ENTRYPOINT ["dotnet", "MasterKinderAPI.dll"]
