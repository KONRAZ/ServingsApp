FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Servings.MockApi.csproj", "."]
RUN dotnet restore "./Servings.MockApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Servings.MockApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Servings.MockApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Servings.MockApi.dll"]
