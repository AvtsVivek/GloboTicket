#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Services/GloboTicket.Services.EventCatalog/GloboTicket.Services.EventCatalog.csproj", "src/Services/GloboTicket.Services.EventCatalog/"]
RUN dotnet restore "src/Services/GloboTicket.Services.EventCatalog/GloboTicket.Services.EventCatalog.csproj"
COPY . .
WORKDIR "/src/src/Services/GloboTicket.Services.EventCatalog"
RUN dotnet build "GloboTicket.Services.EventCatalog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GloboTicket.Services.EventCatalog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GloboTicket.Services.EventCatalog.dll"]
