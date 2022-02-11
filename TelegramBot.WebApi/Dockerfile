
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["TelegramBot.WebApi/TelegramBot.Service.csproj", "TelegramBot.WebApi/"]
COPY ["TelegramBot.BLL/TelegramBot.BLL.csproj", "TelegramBot.BLL/"]
COPY ["TelegramBot.DAL/TelegramBot.DAL.csproj", "TelegramBot.DAL/"]
RUN dotnet restore "TelegramBot.WebApi/TelegramBot.Service.csproj"
COPY . .
WORKDIR "/src/TelegramBot.WebApi"
RUN dotnet build "TelegramBot.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelegramBot.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelegramBot.Service.dll"]
