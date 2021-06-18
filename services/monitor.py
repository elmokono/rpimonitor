#sensors monitor - elmokono@hotmail.com
#-----------------------------------------
#https://www.raspberrypi.org/documentation/linux/software/python.md
#https://learn.adafruit.com/dht-humidity-sensing-on-raspberry-pi-with-gdocs-logging/python-setup
#https://pymongo.readthedocs.io/en/stable/installation.html
#https://www.raspberrypi.org/documentation/usage/gpio/python/README.md

#import Adafruit_DHT
import board
import adafruit_dht
from gpiozero import LED
import pymongo
from datetime import datetime
import requests
import json

#leds
error_led = LED(23)
info_led = LED(22)

#dht11
dhtDevice = adafruit_dht.DHT11(board.D17)
temperature = dhtDevice.temperature
humidity = dhtDevice.humidity

if humidity is not None and temperature is not None:
   print('Temp={0:0.1f}*C  Humidity={1:0.1f}%'.format(temperature, humidity))
else:
   print('Failed to get reading. Try again!')

#mongodb connection
myclient = pymongo.MongoClient("mongodb://127.0.0.1:27017/")
mydb = myclient["monitor"]
mycol = mydb["metrics"]

#openweather api (pilar)
info_led.on()
ow_file = open("./openweather.secret", "r")
ow_secret = ow_file.readline().replace('\n','')
ow_api_url = 'http://api.openweathermap.org/data/2.5/weather?q=Pilar,AR&units=metrics&appid=' + ow_secret
ow_response = requests.get(ow_api_url)
ow_data_dict = ow_response.json()
ow_temp = 0.0
ow_humi = 0.0

for ow_key in ow_data_dict:
        if (ow_key == "main"):
                ow_temp = ow_data_dict[ow_key]["temp"] - 273.15
                ow_humi = ow_data_dict[ow_key]["humidity"]

print('ow temp:{0}, humi:{1}'.format(ow_temp,ow_humi))

tempDict = {
    "timestamp" : datetime.now().strftime("%Y-%m-%d %H:%M:%S"),
    "type" : "temp",
    "value" : temperature,
    "zoneValue" : ow_temp
}

humiDict = {
    "timestamp" : datetime.now().strftime("%Y-%m-%d %H:%M:%S"),
    "type" : "humi",
    "value" : humidity,
    "zoneValue" : ow_humi
}

mycol.insert_one(tempDict)
mycol.insert_one(humiDict)
info_led.off()

dhtDevice.exit()

