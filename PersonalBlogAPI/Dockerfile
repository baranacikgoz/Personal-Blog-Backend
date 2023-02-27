# Use the official .NET 7 SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set the working directory to /app
WORKDIR /app

# Copy the .csproj files and restore dependencies
COPY *.sln .
COPY src/PersonalBlog.Domain/*.csproj ./src/PersonalBlog.Domain/
COPY src/PersonalBlog.Application/*.csproj ./src/PersonalBlog.Application/
COPY src/PersonalBlog.Infrastructure/*.csproj ./src/PersonalBlog.Infrastructure/
COPY src/PersonalBlog.WebAPI/*.csproj ./src/PersonalBlog.WebAPI/
RUN dotnet restore

# Copy the remaining source code and build the application
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official runtime image for .NET 7
FROM mcr.microsoft.com/dotnet/aspnet:7.0

# Set the working directory to /app
WORKDIR /app

# Copy the built application from the build environment
COPY --from=build-env /app/out .

# Set the entry point to run the Web API DLL file
ENTRYPOINT ["dotnet", "PersonalBlog.WebAPI.dll"]