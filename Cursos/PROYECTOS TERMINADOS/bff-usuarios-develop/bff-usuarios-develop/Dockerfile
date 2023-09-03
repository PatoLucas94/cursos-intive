#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM docker-spv.artifactory.gscorp.ad/aspnet-netcore-spv:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM docker-spv.artifactory.gscorp.ad/sdk-netcore-spv:3.1 AS build
WORKDIR /src
COPY ["Spv.Usuarios.Bff/Spv.Usuarios.Bff.csproj", "Spv.Usuarios.Bff/"]
COPY ["Spv.Usuarios.Bff.Service.Interface/Spv.Usuarios.Bff.Service.Interface.csproj", "Spv.Usuarios.Bff.Service.Interface/"]
COPY ["Spv.Usuarios.Bff.Domain/Spv.Usuarios.Bff.Domain.csproj", "Spv.Usuarios.Bff.Domain/"]
#COPY ["Spv.Usuarios.DataAccess.Interface/Spv.Usuarios.DataAccess.Interface.csproj", "Spv.Usuarios.DataAccess.Interface/"]
#COPY ["Spv.Usuarios.DataAccess.ExternalWebService.Interfaces/Spv.Usuarios.DataAccess.ExternalWebService.Interfaces.csproj", "Spv.Usuarios.DataAccess.ExternalWebService.Interfaces/"]
COPY ["Spv.Usuarios.Bff.Common/Spv.Usuarios.Bff.Common.csproj", "Spv.Usuarios.Bff.Common/"]
#COPY ["Spv.Usuarios.DataAccess.ExternalWebService/Spv.Usuarios.DataAccess.ExternalWebService.csproj", "Spv.Usuarios.DataAccess.ExternalWebService/"]
#COPY ["Spv.Usuarios.DataAccess.EntityFramework/Spv.Usuarios.DataAccess.EntityFramework.csproj", "Spv.Usuarios.DataAccess.EntityFramework/"]
COPY ["Spv.Usuarios.Bff.Service/Spv.Usuarios.Bff.Service.csproj", "Spv.Usuarios.Bff.Service/"]
RUN dotnet restore "Spv.Usuarios.Bff/Spv.Usuarios.Bff.csproj"
COPY . .
WORKDIR "/src/Spv.Usuarios.Bff"
RUN dotnet build "Spv.Usuarios.Bff.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spv.Usuarios.Bff.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spv.Usuarios.Bff.dll"]