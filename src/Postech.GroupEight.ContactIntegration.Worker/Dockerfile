FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 5678 # Porta do Prometheus 

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Postech.GroupEight.ContactIntegration.Worker/Postech.GroupEight.ContactIntegration.Worker.csproj", "src/Postech.GroupEight.ContactIntegration.Worker/"]
RUN dotnet restore "src/Postech.GroupEight.ContactIntegration.Worker/Postech.GroupEight.ContactIntegration.Worker.csproj"
COPY . .
WORKDIR "/src/src/Postech.GroupEight.ContactIntegration.Worker"
RUN dotnet build "Postech.GroupEight.ContactIntegration.Worker.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Postech.GroupEight.ContactIntegration.Worker.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM build AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Postech.GroupEight.ContactIntegration.Worker.dll"]