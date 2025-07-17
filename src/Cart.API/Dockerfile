FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Cart.API/Cart.API.csproj", "src/Cart.API/"]
RUN dotnet restore "Cart.API\Cart.API.csproj"
COPY . .
WORKDIR "/src/Cart.API"
RUN dotnet build "Cart.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Cart.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cart.API.dll"]
