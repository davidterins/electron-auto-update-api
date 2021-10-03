# dotnet core electric Updates API

## Release to containerized Api to Heroku

### Requirements

- Heroku CLI
- Heroku account
- Existing Heroku app project (electric-updates-api)
  
### Quick publish to heroku

Run powershell script `./ElectronAutoUpdateApi/heroku-publish/heroku-publish.ps1`

### Publish containerized project to Heroku manualy

1. Publish electricUpdatesApi e.g. `dotnet publish -c release`, do it through VS solution explorer.
2. Place a Dockerfile inside the publish directory, with content:

```dockerfile
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY . .
CMD ASPNETCORE_URLS=http://*:$PORT APIKey=$APIKey dotnet electricUpdatesAPI.dll
```

3. Build and publish image to heroku
   1. From the publish directory run `docker build -t electric-updates-api .` to build the image.
   2. Tag the image: `docker tag electric-updates-api registry.heroku.com/electric-updates-api/web`
   3. Push the image to heroku registry: `docker push registry.heroku.com/electric-updates-api/web`
4. Go live with heroku CLI.
   1. Login `heroku login`
   2. Release `heroku container:release web -a electric-updates-api`
