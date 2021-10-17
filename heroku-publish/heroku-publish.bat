dotnet publish ../ElectronAutoUpdateApi/ElectronAutoUpdateApi.csproj -c release -o ../ElectronAutoUpdateApi/bin/publish
docker build -t logspace-auto-update-api ../ElectronAutoUpdateApi/bin/
docker tag logspace-auto-update-api registry.heroku.com/logspace-auto-update-api/web
docker push registry.heroku.com/logspace-auto-update-api/web
heroku container:release web -a logspace-auto-update-api