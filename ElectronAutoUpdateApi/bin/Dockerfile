FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS runtime
WORKDIR /app
COPY ./publish/ .
CMD ASPNETCORE_URLS=http://*:$PORT APIKey=$APIKey dotnet ElectronAutoUpdateApi.dll
