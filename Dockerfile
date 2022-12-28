# Start from the .NET 6 SDK image
FROM mcr.microsoft.com/dotnet/sdk:7.0

# Install node using apt-get
RUN apt-get update && apt-get install -y nodejs

# Set the working directory to the root solution folder
WORKDIR /build

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
RUN dotnet publish src/NetSendGridEmailClient.Web/NetSendGridEmailClient.Web.csproj -c Release -o /publish

# Remove the build folder
RUN rm -rf /build

# Set the working directory to the output directory
WORKDIR /publish
EXPOSE 443
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "NetSendGridEmailClient.Web.dll"]