FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /WebAppReceive

# Copy only the project file first to restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the entire application code
COPY . ./

RUN dotnet add package Npgsql --version 5.0.12
RUN dotnet add package RabbitMQ.Client --version 6.8.1
RUN dotnet add package Newtonsoft.Json --version 13.0.3

RUN dotnet build -c Release -o out

# Publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /WebAppReceive
COPY --from=build-env /WebAppReceive/out .

# Expose the port that your web application listens on
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["dotnet", "WebAppReceive.dll"]
