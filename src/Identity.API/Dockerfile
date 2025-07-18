FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Identity.API/Identity.API.csproj", "src/Identity.API/"]
RUN dotnet restore "src\Identity.API\Identity.API.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "src\Identity.API\Identity.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "src\Identity.API\Identity.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]
