FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY ./*sln ./

COPY ./OpenFTTH.APIGateway/*.csproj ./OpenFTTH.APIGateway/
COPY ./OpenFTTH.APIGateway.RouteNetwork/*.csproj ./OpenFTTH.APIGateway.RouteNetwork/

RUN dotnet restore --packages ./packages

COPY . ./
WORKDIR /app/OpenFTTH.APIGateway
RUN dotnet publish -c Release -o out --packages ./packages

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app

COPY --from=build-env /app/OpenFTTH.APIGateway/out .
ENTRYPOINT ["dotnet", "OpenFTTH.APIGateway.dll"]

ENV ASPNETCORE_URLS=http://+80
EXPOSE 80
