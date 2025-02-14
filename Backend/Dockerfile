FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiera solution och projekt filer först
COPY ["Backend/Backend.csproj", "Backend/"]
COPY ["Backend.Test/Backend.Test.csproj", "Backend.Test/"]
COPY ["Wizardworks_TBAS.sln", "./"]
COPY [".env", "./"]
RUN dotnet restore "Backend/Backend.csproj"

# Kopiera resten av backend-koden
COPY Backend/. Backend/
WORKDIR "/src/Backend"
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5129
ENV ASPNETCORE_URLS=http://+:5129
ENTRYPOINT ["dotnet", "Backend.dll"]