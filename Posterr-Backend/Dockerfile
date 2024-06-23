# Use the .NET 8 SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution file and restore as distinct layers
COPY Posterr.sln ./
COPY Application/Application.csproj Application/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Domain/Domain.csproj Domain/
COPY API/API.csproj API/
RUN dotnet restore API/API.csproj

# Copy everything else and build the app
COPY . .
WORKDIR /app/API

RUN dotnet build API.csproj -c Release -o /app/build
      
# Publish the app
RUN dotnet publish API.csproj -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

# Set the environment variable for ASP.NET Core
ENV ASPNETCORE_ENVIRONMENT Development

# Expose the port that the app listens on
EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]
