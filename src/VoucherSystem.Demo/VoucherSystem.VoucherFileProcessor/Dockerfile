#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VoucherSystem.VoucherFileProcessor/VoucherSystem.VoucherFileProcessor.csproj", "VoucherSystem.VoucherFileProcessor/"]
RUN dotnet restore "VoucherSystem.VoucherFileProcessor/VoucherSystem.VoucherFileProcessor.csproj"
COPY . .
WORKDIR "/src/VoucherSystem.VoucherFileProcessor"
RUN dotnet build "VoucherSystem.VoucherFileProcessor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VoucherSystem.VoucherFileProcessor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VoucherSystem.VoucherFileProcessor.dll"]