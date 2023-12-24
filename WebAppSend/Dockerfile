FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /WebAppSend

COPY *.csproj ./
RUN dotnet restore

COPY . ./

RUN dotnet add package Npgsql --version 5.0.12
RUN dotnet add package RabbitMQ.Client --version 6.8.1
RUN dotnet add package Newtonsoft.Json --version 13.0.3

RUN dotnet build -c Release -o out

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /WebAppSend
COPY --from=build-env /WebAppSend/out .

EXPOSE 8081
ENV ASPNETCORE_URLS=http://*:8081

ENTRYPOINT ["dotnet", "WebAppSend.dll"]
