#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/nightly/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/nightly/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/{{pascal}}Service.Application/{{pascal}}Service.Application.csproj", "{{pascal}}Service.Application/"]
RUN dotnet restore "{{pascal}}Service.Application/{{pascal}}Service.Application.csproj"
COPY /src .
WORKDIR "/src/{{pascal}}Service.Application"
RUN dotnet build "{{pascal}}Service.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "{{pascal}}Service.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "{{pascal}}Service.Application.dll"]