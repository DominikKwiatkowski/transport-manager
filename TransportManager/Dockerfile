#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TransportManager/TransportManager.csproj", "TransportManager/"]
RUN dotnet restore "TransportManager/TransportManager.csproj"
COPY . .
WORKDIR "/src/TransportManager"
RUN dotnet build "TransportManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TransportManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransportManager.dll"]