# Use the .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the code and build
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Use the runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

# Set the port for Render
ENV PORT=8080
ENV ASPNETCORE_URLS=http://+:${PORT}

# Start the app (CHANGE 'YourProjectName' to the name of your .csproj file without the .csproj extension!)
ENTRYPOINT ["dotnet", "samstore-api.dll"]