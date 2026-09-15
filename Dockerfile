FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/CarRental.Domain/CarRental.Domain.csproj src/CarRental.Domain/
COPY src/CarRental.Application/CarRental.Application.csproj src/CarRental.Application/
COPY src/CarRental.Infrastructure/CarRental.Infrastructure.csproj src/CarRental.Infrastructure/
COPY src/CarRental.Api/CarRental.Api.csproj src/CarRental.Api/
RUN dotnet restore src/CarRental.Api/CarRental.Api.csproj

COPY src/ src/
RUN dotnet publish src/CarRental.Api/CarRental.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN useradd --system --no-create-home --shell /usr/sbin/nologin appuser

COPY --from=build --chown=appuser:appuser /app/publish .

USER appuser

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CarRental.Api.dll"]
