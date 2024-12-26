# ---------------------------------------------------------
# 1) Base runtime image
# ---------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# If you want to run as a non-root user (from your original file)
USER $APP_UID

WORKDIR /app

# Expose ports 8080 & 8443
EXPOSE 8080
EXPOSE 8443

# ---------------------------------------------------------
# 2) Build stage with SDK
# ---------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Optional: specify build configuration via ARG
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy and restore csproj
COPY ["DotaProject/DotaProject.csproj", "DotaProject/"]
RUN dotnet restore "DotaProject/DotaProject.csproj"

# Copy the rest of your source code
COPY . .
WORKDIR "/src/DotaProject"

# Build the application
RUN dotnet build "DotaProject.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ---------------------------------------------------------
# 3) Generate SSL certificate (still in the build stage)
# ---------------------------------------------------------
RUN apt-get update && apt-get install -y openssl && \
    mkdir -p /app/certs && \
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
      -keyout /app/certs/server.key -out /app/certs/server.crt \
      -subj "/CN=localhost" && \
    openssl pkcs12 -export -out /app/certs/certificate.pfx \
      -inkey /app/certs/server.key -in /app/certs/server.crt \
      -password pass:YourSecurePassword

# ---------------------------------------------------------
# 4) Publish stage
# ---------------------------------------------------------
FROM build AS publish

# Use the same ARG for consistency
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "DotaProject.csproj" -c $BUILD_CONFIGURATION \
    -o /app/publish /p:UseAppHost=false

# ---------------------------------------------------------
# 5) Final runtime image
# ---------------------------------------------------------
FROM base AS final

WORKDIR /app

# Create certs directory in final image
RUN mkdir -p /app/certs

# Copy the certificate from the build stage
COPY --from=build /app/certs/certificate.pfx /app/certs/

# Copy published app from publish stage
COPY --from=publish /app/publish .

# Configure HTTPS environment variables
ENV ASPNETCORE_URLS="http://+:8080;https://+:8443" \
    ASPNETCORE_Kestrel__Certificates__Default__Path=/app/certs/certificate.pfx \
    ASPNETCORE_Kestrel__Certificates__Default__Password=YourSecurePassword

# Start the .NET app
ENTRYPOINT ["dotnet", "DotaProject.dll"]
