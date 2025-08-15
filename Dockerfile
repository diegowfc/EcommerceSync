# ===== build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copie primeiro o .csproj para cache eficiente
COPY ./src/WebAPI/WebAPI.csproj ./src/WebAPI/
RUN dotnet restore ./src/WebAPI/WebAPI.csproj

# agora copie o restante do código
COPY . .
RUN dotnet publish ./src/WebAPI/WebAPI.csproj -c Release -o /out

# ===== runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

# Render injeta $PORT; bind explícito em 0.0.0.0
# se o nome do seu .csproj for WebAPI.csproj, a DLL é WebAPI.dll
CMD ["sh", "-c", "dotnet WebAPI.dll --urls http://0.0.0.0:$PORT"]
