# Electron Auto Update API

An API to be used together with [electron-updater's](github.com/Squirrel/Squirrel.Windows/blob/develop/src/Squirrel/UpdateManager.cs) "Generic" provider in your Electron application to make auto updates easier & more custmizable in both development and production environments.

Checkout [auto-update-electron-app](https://github.com/davidterins/auto-update-electron-app) to see how to configure your Electron app to use the API as a "Generic" provider.

## Configuration

In `./ElectronAutoUpdateApi/appsettings.json` & `./ElectronAutoUpdateApi/appsettings.Development.json` update the following section to the GitHub repository to where you are storing your application's releases.

```json
  "Github": {
    "Private": "false",
    "Owner": "davidterins",
    "Repository": "my-apps-updates-repository",
    "ProductHeader": "AProductHeader"
  }
```

If using the API for a private repository and want to avoid specifying your GitHub token within the source code, run:

```dotnet
dotnet user-secrets set "APIKey" "YourGHToken"
```

## Development

Just start the application. Debug and test your endpoints with Swagger https://localhost:5004/swagger/.

## Deploy the api as a containerized application to Heroku

### Requirements

- Heroku account
- Heroku CLI installed
- Docker installed

### Deploy with Heroku CLI 🚀

1. Run `heroku login`
2. Run `heroku container:login`
3. Run `heroku apps:create nameOfYourApp`
4. Modify `./heroku-publish/heroku-publish.bat` by replacing occurances of "nameOfYourApp" to the name used in step 3.
5. Finally run `./heroku-publish/heroku-publish.bat`.
6. Checkout your API at https://nameOfYourApp.herokuapp.com/api
