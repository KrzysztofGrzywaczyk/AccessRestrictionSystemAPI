FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine3.16 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install cultures (same approach as Alpine SDK image)
RUN apk add --no-cache icu-libs

# Disable the invariant mode (set in base image)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.16 AS build
COPY ["src/AccessControlSystem.Api/AccessControlSystem.Api.csproj", "AccessControlSystem.Api/"]
COPY ["src/AccessControlSystem.SharedKernel/AccessControlSystem.SharedKernel.csproj", "AccessControlSystem.SharedKernel/"]
RUN dotnet restore "AccessControlSystem.Api/AccessControlSystem.Api.csproj"
RUN dotnet restore "AccessControlSystem.SharedKernel/AccessControlSystem.SharedKernel.csproj"
COPY . .
WORKDIR "/src/AccessControlSystem.Api"
RUN dotnet build "AccessControlSystem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccessControlSystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccessControlSystem.Api.dll"]