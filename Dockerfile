FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# criar uma pasta dentro do container e copiar os arquivos
RUN mkdir /app
WORKDIR /app
COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o out

# build da aplicação
FROM mcr.microsoft.com/dotnet/aspnet:7.0

# copiando o binario gerado para o container
COPY --from=build-env /app/out .

# setar as variaveis de ambiente
ENV tokem=1820018763:AAHTTC5m_AvjaGoo8_sinIfTZ0HHRW3HK2c
ENV opemAItokem=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

# iniciar a aplicação
ENTRYPOINT ["dotnet", "opemainet.dll"]
