# Start from the .NET 6 SDK image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 as build

# Install node using apt-get
RUN apt-get update && apt-get install -y nodejs

# Copy the entire solution folder into the container
COPY . .

# Restore dependencies and build the projects
RUN dotnet restore src/NetSendGridEmailClient.Interface/NetSendGridEmailClient.Interface.csproj
RUN dotnet build src/NetSendGridEmailClient.Interface/NetSendGridEmailClient.Interface.csproj

RUN dotnet restore src/NetSendGridEmailClient.Models/NetSendGridEmailClient.Models.csproj
RUN dotnet build src/NetSendGridEmailClient.Models/NetSendGridEmailClient.Models.csproj

RUN dotnet restore src/NetSendGridEmailClient.Functions/NetSendGridEmailClient.Functions.csproj
RUN dotnet build src/NetSendGridEmailClient.Functions/NetSendGridEmailClient.Functions.csproj

RUN dotnet restore src/NetSendGridEmailClient.Services/NetSendGridEmailClient.Services.csproj
RUN dotnet build src/NetSendGridEmailClient.Services/NetSendGridEmailClient.Services.csproj

RUN dotnet restore src/NetSendGridEmailClient.Web/NetSendGridEmailClient.Web.csproj
RUN dotnet build src/NetSendGridEmailClient.Web/NetSendGridEmailClient.Web.csproj

# Public the runtime project
FROM build AS publish
RUN dotnet publish src/NetSendGridEmailClient.Web/NetSendGridEmailClient.Web.csproj -c Release -o /app/publish

# Set the working directory to the output directory
FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

# Run the application
ENTRYPOINT ["dotnet", "NetSendGridEmailClient.Web.dll"]
