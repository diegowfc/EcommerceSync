# ===== build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia tudo (mais simples; menos cache, mas funciona em qualquer layout)
COPY . .

# RESTORE e PUBLISH apontando para o .csproj CORRETO (pasta WebApi)
RUN dotnet restore ./src/WebApi/WebAPI.csproj
RUN dotnet publish ./src/WebApi/WebAPI.csproj -c Release -o /out

# ===== runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

# Render injeta $PORT; faça o bind em 0.0.0.0
# ATENÇÃO: troque o nome da DLL se for diferente
CMD ["sh", "-c", "dotnet WebAPI.dll --urls http://0.0.0.0:$PORT"]
