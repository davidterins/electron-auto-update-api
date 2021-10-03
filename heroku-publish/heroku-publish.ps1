dotnet publish ../ElectronAutoUpdateApi.csproj -c release -o ../bin/publish
docker build -t electric-updates-api
docker tag electric-updates-api registry.heroku.com/electric-updates-api/web
docker push registry.heroku.com/electric-updates-api/web
heroku container:release web -a electric-updates-api