# ESTÁGIO 1: Build
# Imagem completa do SDK do .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de projeto primeiro para otimizar o cache do Docker
COPY ["RelatorioApi.csproj", "./"]
RUN dotnet restore "RelatorioApi.csproj"

# Copia o restante do código e compila a versão final
COPY . .
RUN dotnet publish "RelatorioApi.csproj" -c Release -o /app/publish

# ESTÁGIO 2: Runtime
# Imagem do ASP.NET, sem compiladores, apenas roda a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia da "fábrica" (estágio build) apenas os arquivos compilados para o estágio final
COPY --from=build /app/publish .

# Diz ao Docker qual comando rodar quando o container ligar
ENTRYPOINT ["dotnet", "RelatorioApi.dll"]