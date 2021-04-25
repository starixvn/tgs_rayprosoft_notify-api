#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BuildConfiguration=Dev
WORKDIR /src
COPY ["NotifyApi/NotifyApi.csproj", "NotifyApi/"]
COPY ["NotifyApi/nuget.config", "NotifyApi/"]
RUN dotnet restore "NotifyApi/NotifyApi.csproj"
COPY . .
WORKDIR "/src/NotifyApi"
RUN dotnet build "NotifyApi.csproj" -c ${BuildConfiguration} -o /app/build

FROM build AS publish
RUN dotnet publish "NotifyApi.csproj" -c ${BuildConfiguration} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TGS.NotifyApi.dll"]