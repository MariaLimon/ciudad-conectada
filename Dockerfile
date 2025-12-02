# Imagen base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# SDK para compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar SOLO el backend
COPY Backend/Backend.csproj Backend/
RUN dotnet restore "Backend/Backend.csproj"

# Copiar todo el backend
COPY Backend/ Backend/

# Copiar la carpeta wwwroot (IMPORTANTE)
COPY Backend/wwwroot Backend/wwwroot

WORKDIR /src/Backend
RUN dotnet build "Backend.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app

# Copiar publish
COPY --from=publish /app/publish .

# Copiar wwwroot al contenedor final
COPY Backend/wwwroot ./wwwroot

ENTRYPOINT ["dotnet", "Backend.dll"]
