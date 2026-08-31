# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only project files first for layer caching
COPY LENA/LENA.Domain.csproj LENA/
COPY LENA.Application/LENA.Application.csproj LENA.Application/
COPY LENA.API/LENA.API.csproj LENA.API/

RUN dotnet restore LENA.API/LENA.API.csproj

# Copy source
COPY LENA/ LENA/
COPY LENA.Application/ LENA.Application/
COPY LENA.API/ LENA.API/

WORKDIR /src/LENA.API
RUN dotnet publish LENA.API.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LENA.API.dll"]
