FROM mcr.microsoft.com/azure-functions/dotnet:3.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AzureAutoscalingToolbox.Samples.StatefulAppInstances.csproj", ""]

COPY . .
WORKDIR "/src/."
RUN dotnet build "AzureAutoscalingToolbox.Samples.StatefulAppInstances.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AzureAutoscalingToolbox.Samples.StatefulAppInstances.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV AzureWebJobsScriptRoot=/app \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true