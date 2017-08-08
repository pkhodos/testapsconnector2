# Fallball connector .Net Core

This is a basic sample connector based on .Net Core Framework for the [Fallball Cloud Storage](https://github.com/ingrammicro/fallball-service).

## Running on localhost with tunnel
* Download and unzip fallball-connector-dotnet
* Open FallballConnectorDotNet.sln using Visual Studio or use following commands:

```bash
dotnet build
dotnet run
```

* Update `config.yml` file with your credentials

```yaml
fallball_service_url: PUT_HERE_FALLBALL_SERVICE_URI
fallball_service_authorization_token: PUT_HERE_FALLBALL_SERVICE_AUTHORIZATION_TOKEN
oauth_key: PUT_HERE_OAUTH_KEY
oauth_secret: PUT_HERE_OAUTH_SECRET
```

* Use URL = "https://hostname/v1" in APS Connect

## Running in Docker

```bash
cd FallballConnectorDotNet
docker build -t  fallballconnectordotnet .
docker run -d -p 8080:80 --name myapp fallballconnectordotnet
```

## Development
* run FallballConnectorDotNet.sln using Visual Studio
