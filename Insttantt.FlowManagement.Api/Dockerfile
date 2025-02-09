#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Insttantt.FlowManagement.Api/Insttantt.FlowManagement.Api.csproj", "Insttantt.FlowManagement.Api/"]
COPY ["Insttant.FlowManagement.Infrastructure/Insttantt.FlowManagement.Infrastructure.csproj", "Insttant.FlowManagement.Infrastructure/"]
COPY ["Insttantt.FlowManagement.Application/Insttantt.FlowManagement.Application.csproj", "Insttantt.FlowManagement.Application/"]
COPY ["Insttantt.FlowManagement.Domain/Insttantt.FlowManagement.Domain.csproj", "Insttantt.FlowManagement.Domain/"]
RUN dotnet restore "./Insttantt.FlowManagement.Api/./Insttantt.FlowManagement.Api.csproj"
COPY . .
WORKDIR "/src/Insttantt.FlowManagement.Api"
RUN dotnet build "./Insttantt.FlowManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Insttantt.FlowManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Insttantt.FlowManagement.Api.dll"]