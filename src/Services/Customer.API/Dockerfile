#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Customer.API/Customer.API.csproj", "src/Services/Customer.API/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "src/BuildingBlocks/Common.Logging/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "src/BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "src/BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "src/BuildingBlocks/Shared/"]
RUN dotnet restore "src/Services/Customer.API/Customer.API.csproj"
COPY . .
WORKDIR "/src/Services/Customer.API"
RUN dotnet build "Customer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Customer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customer.API.dll"]