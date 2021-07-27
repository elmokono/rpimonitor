#!/bin/bash

CONTAINER_NAME="homemonitor"

#start
docker container stop $CONTAINER_NAME
docker container rm $CONTAINER_NAME
docker run --name $CONTAINER_NAME --restart unless-stopped -p 80:80 $CONTAINER_NAME

