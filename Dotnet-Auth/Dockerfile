
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Dotnet-Auth/Dotnet-Auth.csproj", "Dotnet-Auth/"]
RUN dotnet restore "Dotnet-Auth/Dotnet-Auth.csproj"
COPY . .
WORKDIR "/src/Dotnet-Auth"
RUN dotnet build "Dotnet-Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dotnet-Auth.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dotnet-Auth.dll"]