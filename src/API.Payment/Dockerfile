#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/API.Payment/API.Payment.csproj", "src/API.Payment/"]
COPY ["src/Application.Base/Application.Base.csproj", "src/Application.Base/"]
COPY ["src/Domain.Base/Domain.Base.csproj", "src/Domain.Base/"]
COPY ["src/Infrastructure.Base/Infrastructure.Base.csproj", "src/Infrastructure.Base/"]
COPY ["src/Plugins/Plugins.Stripe.StripeLibrary/Plugin.Stripe.StripeLibrary.csproj", "src/Plugins/Plugins.Stripe.StripeLibrary/"]
COPY ["src/Plugins/Plugin.Stripe/Plugin.Stripe.csproj", "src/Plugins/Plugin.Stripe/"]
COPY ["src/Base.API/Base.API.csproj", "src/Base.API/"]
COPY ["src/Application.CommunicationStandard/Service.CommunicationStandard.csproj", "src/Application.CommunicationStandard/"]
RUN dotnet restore "src/API.Payment/API.Payment.csproj"
COPY . .
WORKDIR "/src/src/API.Payment"
RUN dotnet build "API.Payment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.Payment.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Payment.dll"]