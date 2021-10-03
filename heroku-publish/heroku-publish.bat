dotnet publish ../ElectronAutoUpdateApi/ElectronAutoUpdateApi.csproj -c release -o ../ElectronAutoUpdateApi/bin/publish
docker build -t electron-auto-update-api ../ElectronAutoUpdateApi/bin/
docker tag electron-auto-update-api registry.heroku.com/electron-auto-update-api/web
docker push registry.heroku.com/electron-auto-update-api/web
heroku container:release web -a electron-auto-update-api