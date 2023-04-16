#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Website.MarketingSite/Website.MarketingSite.csproj", "src/Website.MarketingSite/"]
COPY ["src/Application.CommunicationStandard/Service.CommunicationStandard.csproj", "src/Application.CommunicationStandard/"]
COPY ["src/Utilities/Utilities.csproj", "src/Utilities/"]
COPY ["src/Utilities.NewtonsoftSerializer/Utilities.NewtonsoftSerializer.csproj", "src/Utilities.NewtonsoftSerializer/"]
RUN dotnet restore "src/Website.MarketingSite/Website.MarketingSite.csproj"
COPY . .
WORKDIR "/src/src/Website.MarketingSite"
RUN dotnet build "Website.MarketingSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Website.MarketingSite.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Website.MarketingSite.dll"]