# Usar una imagen base de ASP.NET para producción
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Usar la imagen base de .NET SDK para la construcción
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CollaborativeBoardApi.csproj", "./"]
RUN dotnet restore "CollaborativeBoardApi.csproj"
WORKDIR "/src"
COPY . .
RUN dotnet build "CollaborativeBoardApi.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "CollaborativeBoardApi.csproj" -c Release -o /app/publish

# Crear la imagen final que se ejecutará
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CollaborativeBoardApi.dll"]
