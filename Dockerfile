# Usa la imagen base de ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Usa la imagen del SDK de .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["InmobiliariaCA/InmobiliariaCA.csproj", "./"]
RUN dotnet restore "./InmobiliariaCA.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "InmobiliariaCA.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "InmobiliariaCA.csproj" -c Release -o /app/publish

# Configurar la imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InmobiliariaCA.dll"]
