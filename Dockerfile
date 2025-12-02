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
WORKDIR /src/Backend
RUN dotnet build "Backend.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]
