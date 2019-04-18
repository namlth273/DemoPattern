FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /qpp
COPY *.sln ./

COPY /*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore
COPY . ./
RUN dotnet publish DemoPattern/DemoPattern.csproj -c Release -o out

FROM base AS final
WORKDIR /app
COPY --from=build /qpp/DemoPattern/out/ .
ENTRYPOINT ["dotnet", "DemoPattern.dll"]

#docker build -t demowebapi .
#docker run -d -p 8002:80 demowebapi