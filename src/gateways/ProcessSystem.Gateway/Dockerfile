#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/gateways/ProcessSystem.Gateway/ProcessSystem.csproj", "src/gateways/ProcessSystem.Gateway/"]
COPY ["src/BuildingBlocks/Common/Common.csproj", "src/BuildingBlocks/Common/"]
RUN dotnet restore "src/gateways/ProcessSystem.Gateway/ProcessSystem.csproj"
COPY . .
WORKDIR "/src/src/gateways/ProcessSystem.Gateway"
RUN dotnet build "ProcessSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProcessSystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProcessSystem.dll"]