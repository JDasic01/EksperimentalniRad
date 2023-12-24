FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /WebApp

# Copy only the project file first to restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the entire application code
COPY . ./

RUN dotnet add package Npgsql --version 5.0.12
RUN dotnet add package Newtonsoft.Json --version 13.0.3

RUN dotnet build -c Release -o out

# Publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /WebApp
COPY --from=build-env /WebApp/out .

# Expose the port that your web application listens on
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

# Set the entry point for the application
ENTRYPOINT ["dotnet", "WebApp.dll"]
