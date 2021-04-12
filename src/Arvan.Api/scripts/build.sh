#!/bin/bash

cd ..

echo -e "Docker build..."
docker build -t arvan:v1 .

echo -e "\nDocker images"
docker images

echo -e "\nDocker networks"
docker network create arvannet

docker ps
docker stop arvan-mongo
docker volume ls
docker volume rm mongodbdata
docker run -d --rm --name arvan-mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=mazdak --network=arvannet mongo
docker run -it -d --rm -p 8080:80 -e MongoOptions:Host=arvan-mongo -e MongoOptions:DbPassword=mazdak --network=arvannet arvan:v1
$SHELL