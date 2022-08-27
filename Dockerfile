#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/RockPaperScissorCygniAPI.Controllers/RockPaperScissorCygniAPI.Controllers.csproj", "src/RockPaperScissorCygniAPI.Controllers/"]
COPY ["src/RockPaperScissorCygniAPI.Services/RockPaperScissorCygniAPI.Services.csproj", "src/RockPaperScissorCygniAPI.Services/"]
COPY ["src/RockPaperScissorCygniAPI.Model/RockPaperScissorCygniAPI.Model.csproj", "src/RockPaperScissorCygniAPI.Model/"]
COPY ["src/RockPaperScissorCygniAPI.DataRepository/RockPaperScissorCygniAPI.DataRepository.csproj", "src/RockPaperScissorCygniAPI.DataRepository/"]
RUN dotnet restore "src/RockPaperScissorCygniAPI.Controllers/RockPaperScissorCygniAPI.Controllers.csproj"
COPY . .
WORKDIR "/src/src/RockPaperScissorCygniAPI.Controllers"
RUN dotnet build "RockPaperScissorCygniAPI.Controllers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RockPaperScissorCygniAPI.Controllers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RockPaperScissorCygniAPI.Controllers.dll"]