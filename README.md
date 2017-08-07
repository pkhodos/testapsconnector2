# Fallball connector

This is a basic sample connector for the [Fallball Cloud Storage](https://github.com/ingrammicro/fallball-service).

## Running on localhost with tunnel
run FallballConnectorDotNet.sln

* Update `config.yml` file with your credentials

```yaml
fallball_service_url: PUT_HERE_FALLBALL_SERVICE_URI
fallball_service_authorization_token: PUT_HERE_FALLBALL_SERVICE_AUTHORIZATION_TOKEN
oauth_key: PUT_HERE_OAUTH_KEY
oauth_secret: PUT_HERE_OAUTH_SECRET

## Running in Docker
Not yet implemented

## Development
run FallballConnectorDotNet.sln
