# Usar la imagen base oficial de .NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Usar la imagen SDK de .NET para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["InmobiliariaCA/InmobiliariaCA.csproj", "./"]
RUN dotnet restore "./InmobiliariaCA.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "InmobiliariaCA.csproj" -c Release -o /app/build/InmobiliariaCA/

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "InmobiliariaCA.csproj" -c Release -o /app/publish/InmobiliariaCA/

# Configurar la imagen final y ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish/InmobiliariaCA/ .
ENTRYPOINT ["dotnet", "InmobiliariaCA.dll"]