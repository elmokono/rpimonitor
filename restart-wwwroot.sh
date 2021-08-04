#!/bin/bash

CONTAINER_NAME="homemonitor"

cd /home/pi/monitor

#stop docker
docker container stop $CONTAINER_NAME
docker container rm $CONTAINER_NAME

#rebuild image
if [ '$1' == 'rebuild' ]
then
	docker build -f Dockerfile .
	docker tag 32a8a9bf7ee1 homemonitor
	#docker pull ghcr.io/elmokono/rpimonitor:master
	#docker tag a5540c259d2f homeMonitor
fi

docker run --name $CONTAINER_NAME --restart unless-stopped -p 80:80 $CONTAINER_NAME

