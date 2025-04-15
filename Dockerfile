# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy sln and csproj files first to cache dependencies
COPY secret-sips-server.sln ./
COPY SecretSipsServer/*.csproj ./SecretSipsServer/
RUN dotnet restore ./secret-sips-server.sln

# Copy the rest and build
COPY . ./
WORKDIR /src/SecretSipsServer
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SecretSipsServer.dll"]
