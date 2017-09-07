FROM microsoft/aspnetcore-build:1.1
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish /app/FallballConnectorDotNet.csproj -c Release -o out

ENTRYPOINT ["dotnet", "out/FallballConnectorDotNet.dll"]