#!/bin/bash

CONTAINER_NAME="rpi3-mongodb3.2"
DB_FOLDER="/home/pi/monitor/database/db"
DB_CONFIG_FOLDER="/home/pi/monitor/database/configdb"

#stop existing instance
docker container stop $CONTAINER_NAME
docker container rm $CONTAINER_NAME
docker run --name $CONTAINER_NAME -v $DB_FOLDER:/data/db -v $DB_CONFIG_FOLDER:/data/configdb lowdef/rpi3-mongodb3.2 mongod --repair

#start mongo
docker container stop $CONTAINER_NAME
docker container rm $CONTAINER_NAME
docker run -d --name $CONTAINER_NAME -v $DB_FOLDER:/data/db -v $DB_CONFIG_FOLDER:/data/configdb -p 27017:27017 -p 28017:28017 lowdef/rpi3-mongodb3.2 mongod --storageEngine mmapv1
