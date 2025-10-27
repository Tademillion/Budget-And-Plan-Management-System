# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["BudgetP.csproj", "./"]
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "BudgetP.dll"]