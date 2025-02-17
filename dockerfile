# .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["Presentation/OnionArch.WebApi/OnionArch.WebApi.csproj", "Presentation/OnionArch.WebApi/"]
COPY ["Core/OnionArch.Application/OnionArch.Application.csproj", "Core/OnionArch.Application/"]
COPY ["Core/OnionArch.Domain/OnionArch.Domain.csproj", "Core/OnionArch.Domain/"]
COPY ["infrastructure/OnionArch.infrastructure/OnionArch.infrastructure.csproj", "infrastructure/OnionArch.infrastructure/"]
COPY ["infrastructure/OnionArch.Persistance/OnionArch.Persistance.csproj", "infrastructure/OnionArch.Persistance/"]
COPY ["SignalR/SignalR.csproj", "SignalR/"]
COPY ["infrastructure/Common/Common/Common.csproj", "infrastructure/Common/Common/"]

RUN dotnet restore "Presentation/OnionArch.WebApi/OnionArch.WebApi.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Presentation/OnionArch.WebApi"
RUN dotnet build "OnionArch.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnionArch.WebApi.csproj" -c Release -o /app/publish

# .NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "OnionArch.WebApi.dll"]