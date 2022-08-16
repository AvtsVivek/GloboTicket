#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/GloboTicket.Services.Ordering/GloboTicket.Services.Ordering.csproj", "src/Services/GloboTicket.Services.Ordering/"]
COPY ["src/Integration/GloboTicket.Integration.Messages/GloboTicket.Integration.Messages.csproj", "src/Integration/GloboTicket.Integration.Messages/"]
COPY ["src/Integration/GloboTicket.Integration.MessagingBus/GloboTicket.Integration.MessagingBus.csproj", "src/Integration/GloboTicket.Integration.MessagingBus/"]
RUN dotnet restore "src/Services/GloboTicket.Services.Ordering/GloboTicket.Services.Ordering.csproj"
COPY . .
WORKDIR "/src/src/Services/GloboTicket.Services.Ordering"
RUN dotnet build "GloboTicket.Services.Ordering.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GloboTicket.Services.Ordering.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GloboTicket.Services.Ordering.dll"]
