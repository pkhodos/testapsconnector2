# Fallball connector .Net Core

This is a basic sample connector based on .Net Core Framework for the [Fallball Cloud Storage](https://github.com/ingrammicro/fallball-service).

## Running on localhost with tunnel
* Download and unzip fallball-connector-dotnet
* Open FallballConnectorDotNet.sln using Visual Studio or use following commands:

## Limitations
* Fallball connector .Net Core supports only sync provisionin.

```bash
dotnet restore *.csproj
dotnet build *.csproj
dotnet run
 * Running on http://localhost:5000/ (Press CTRL+C to quit)
```

* Update `config.yml` file with your credentials

```yaml
fallball_service_url: PUT_HERE_FALLBALL_SERVICE_URI
fallball_service_authorization_token: PUT_HERE_FALLBALL_SERVICE_AUTHORIZATION_TOKEN
oauth_key: PUT_HERE_OAUTH_KEY
oauth_secret: PUT_HERE_OAUTH_SECRET
```

* Use URL = "https://<hostname>/connector" in APS Connect
* Create HTTP tunnel with [ngrok](https://ngrok.io)

```bash
ngrok http 5000
```

* Use public connector URL <https://YOUR_UNIQ_ID.ngrok.io/v1/>

If you run the connector without SSL behind SSL-enabled reverse proxy, make sure that proxy populates the `X-Forwarded-Proto` header.


## Running in Docker

```bash
docker build -t fallballconnectordotnet .
docker run -d -p 8080:80 --name myapp fallballconnectordotnet
```

## Development
* run FallballConnectorDotNet.csproj using Visual Studio 2017
