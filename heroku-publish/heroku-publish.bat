dotnet publish ../ElectronAutoUpdateApi/ElectronAutoUpdateApi.csproj -c release -o ../ElectronAutoUpdateApi/bin/publish
docker build -t nameOfYourApp ../ElectronAutoUpdateApi/bin/
docker tag nameOfYourApp registry.heroku.com/nameOfYourApp/web
docker push registry.heroku.com/nameOfYourApp/web
heroku container:release web -a nameOfYourApp