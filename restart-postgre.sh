#!/bin/bash

CONTAINER_NAME="postgres"
DB_FOLDER="/home/pi/monitor/database/postgres/data"

#start
docker container stop $CONTAINER_NAME
docker container rm $CONTAINER_NAME
docker run --name postgres --restart unless-stopped \
-p 5432:5432 \
-e POSTGRES_PASSWORD=yayayaya \
-e PGDATA=/var/lib/postgresql/data/pgdata \
-v $DB_FOLDER:/var/lib/postgresql/data \
-d arm32v7/postgres
