# Use the .NET 8 SDK to build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project file and restore
COPY *.csproj ./
RUN dotnet restore

# Copy rest of code and publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Use the .NET 8 Runtime to run (Notice it says aspnet:8.0)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

# Set the port Render requires
ENV PORT=8080
ENV ASPNETCORE_URLS=http://+:${PORT}

# Run the app (Make sure this matches your file name!)
ENTRYPOINT ["dotnet", "samstore-api.dll"]