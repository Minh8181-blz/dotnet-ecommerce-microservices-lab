#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/API.Identity/API.Identity.csproj", "src/API.Identity/"]
COPY ["src/Application.Base/Application.Base.csproj", "src/Application.Base/"]
COPY ["src/Domain.Base/Domain.Base.csproj", "src/Domain.Base/"]
COPY ["src/Infrastructure.Base/Infrastructure.Base.csproj", "src/Infrastructure.Base/"]
COPY ["src/Base.API/Base.API.csproj", "src/Base.API/"]
COPY ["src/Application.CommunicationStandard/Service.CommunicationStandard.csproj", "src/Application.CommunicationStandard/"]
RUN dotnet restore "src/API.Identity/API.Identity.csproj"
COPY . .
WORKDIR "/src/src/API.Identity"
RUN dotnet build "API.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Identity.dll"]