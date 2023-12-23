FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /WebApp

# Copy only the project file first to restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the entire application code
COPY . ./

RUN dotnet build -c Release -o out

# Publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /WebApp
COPY --from=build-env /WebApp/out .

# Expose the port that your web application listens on
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["dotnet", "WebApp.dll"]
