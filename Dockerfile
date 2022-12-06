#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0-alpine
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["translateBot.csproj", "."]
COPY ["TranslateGoogle/TranslateGoogle.csproj", "TranslateGoogle/"]
RUN dotnet restore "./translateBot.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "translateBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "translateBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "translateBot.dll"]