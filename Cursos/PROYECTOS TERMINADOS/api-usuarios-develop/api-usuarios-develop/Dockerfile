#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM docker-spv.artifactory.gscorp.ad/aspnet-netcore-spv:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM docker-spv.artifactory.gscorp.ad/sdk-netcore-spv:3.1 AS build
WORKDIR /src
COPY ["Spv.Usuarios.Api/Spv.Usuarios.Api.csproj", "Spv.Usuarios.Api/"]
COPY ["Spv.Usuarios.Service.Interface/Spv.Usuarios.Service.Interface.csproj", "Spv.Usuarios.Service.Interface/"]
COPY ["Spv.Usuarios.Domain/Spv.Usuarios.Domain.csproj", "Spv.Usuarios.Domain/"]
COPY ["Spv.Usuarios.DataAccess.Interface/Spv.Usuarios.DataAccess.Interface.csproj", "Spv.Usuarios.DataAccess.Interface/"]
COPY ["Spv.Usuarios.DataAccess.ExternalWebService.Interfaces/Spv.Usuarios.DataAccess.ExternalWebService.Interfaces.csproj", "Spv.Usuarios.DataAccess.ExternalWebService.Interfaces/"]
COPY ["Spv.Usuarios.Common/Spv.Usuarios.Common.csproj", "Spv.Usuarios.Common/"]
COPY ["Spv.Usuarios.DataAccess.ExternalWebService/Spv.Usuarios.DataAccess.ExternalWebService.csproj", "Spv.Usuarios.DataAccess.ExternalWebService/"]
COPY ["Spv.Usuarios.DataAccess.EntityFramework/Spv.Usuarios.DataAccess.EntityFramework.csproj", "Spv.Usuarios.DataAccess.EntityFramework/"]
COPY ["Spv.Usuarios.Service/Spv.Usuarios.Service.csproj", "Spv.Usuarios.Service/"]
RUN dotnet restore "Spv.Usuarios.Api/Spv.Usuarios.Api.csproj"
COPY . .
WORKDIR "/src/Spv.Usuarios.Api"
RUN dotnet build "Spv.Usuarios.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spv.Usuarios.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spv.Usuarios.Api.dll"]