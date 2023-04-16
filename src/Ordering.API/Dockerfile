#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Ordering.API/API.Ordering.csproj", "src/Ordering.API/"]
COPY ["src/Application.Base/Application.Base.csproj", "src/Application.Base/"]
COPY ["src/Domain.Base/Domain.Base.csproj", "src/Domain.Base/"]
COPY ["src/Infrastructure.Base/Infrastructure.Base.csproj", "src/Infrastructure.Base/"]
COPY ["src/Base.API/Base.API.csproj", "src/Base.API/"]
COPY ["src/Application.CommunicationStandard/Service.CommunicationStandard.csproj", "src/Application.CommunicationStandard/"]
COPY ["src/Utilities/Utilities.csproj", "src/Utilities/"]
COPY ["src/BuildingBlocks/BB.EventBus.RabbitMQ/BB.EventBus.RabbitMQ.csproj", "src/BuildingBlocks/BB.EventBus.RabbitMQ/"]
COPY ["src/BuildingBlocks/BB.EventBus/BB.EventBus.csproj", "src/BuildingBlocks/BB.EventBus/"]
COPY ["src/Utilities.NewtonsoftSerializer/Utilities.NewtonsoftSerializer.csproj", "src/Utilities.NewtonsoftSerializer/"]
RUN dotnet restore "src/Ordering.API/API.Ordering.csproj"
COPY . .
WORKDIR "/src/src/Ordering.API"
RUN dotnet build "API.Ordering.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.Ordering.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Ordering.dll"]