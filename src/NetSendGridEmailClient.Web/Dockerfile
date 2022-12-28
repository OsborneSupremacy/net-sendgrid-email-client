#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NetSendGridEmailClient.Web/NetSendGridEmailClient.Web.csproj", "src/NetSendGridEmailClient.Web/"]
COPY ["NetSendGridEmailClient.Functions/NetSendGridEmailClient.Functions.csproj", "src/NetSendGridEmailClient.Functions/"]
COPY ["NetSendGridEmailClient.Models/NetSendGridEmailClient.Models.csproj", "src/NetSendGridEmailClient.Models/"]
COPY ["NetSendGridEmailClient.Interface/NetSendGridEmailClient.Interface.csproj", "src/NetSendGridEmailClient.Interface/"]
COPY ["NetSendGridEmailClient.Services/NetSendGridEmailClient.Services.csproj", "src/NetSendGridEmailClient.Services/"]
RUN dotnet restore "NetSendGridEmailClient.Web/NetSendGridEmailClient.Web.csproj"
COPY . .
WORKDIR "src/NetSendGridEmailClient.Web"
RUN dotnet build "NetSendGridEmailClient.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetSendGridEmailClient.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetSendGridEmailClient.Web.dll"]