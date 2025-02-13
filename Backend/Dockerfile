FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiera csproj och återställ som ett separat lager
COPY *.csproj .
RUN dotnet restore

# Kopiera resten av filerna och bygg
COPY . .
RUN dotnet publish -c Release -o /app

# Bygg runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "YourBackendApp.dll"]
