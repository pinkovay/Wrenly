FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar os arquivos de projeto (.csproj) primeiro para aproveitar o cache de camadas
COPY ["apps/api/Wrenly.Api/Wrenly.Api.csproj", "apps/api/Wrenly.Api/"]
COPY ["apps/api/Wrenly.Domain/Wrenly.Domain.csproj", "apps/api/Wrenly.Domain/"]
COPY ["apps/api/Wrenly.Infrastructure/Wrenly.Infrastructure.csproj", "apps/api/Wrenly.Infrastructure/"]

# Restaurar as dependências
RUN dotnet restore "apps/api/Wrenly.Api/Wrenly.Api.csproj"

# Copiar todo o resto e buildar
COPY . .
WORKDIR "/app/apps/api/Wrenly.Api"
RUN dotnet build "Wrenly.Api.csproj" -c Release -o /app/build

# Estágio de Publicação
FROM build AS publish
RUN dotnet publish "Wrenly.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio Final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wrenly.Api.dll"]