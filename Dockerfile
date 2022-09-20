#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UPOD.API/UPOD.API.csproj", "UPOD.API/"]
COPY ["UPOD.REPOSITORIES/UPOD.REPOSITORIES.csproj", "UPOD.REPOSITORIES/"]
COPY ["UPOD.SERVICES/UPOD.SERVICES.csproj", "UPOD.SERVICES/"]
RUN dotnet restore "UPOD.API/UPOD.API.csproj"
COPY . .
WORKDIR "/src/UPOD.API"
RUN dotnet build "UPOD.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UPOD.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UPOD.API.dll"]