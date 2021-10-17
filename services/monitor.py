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
#import pymongo
import psycopg2
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
#myclient = pymongo.MongoClient("mongodb://127.0.0.1:27017/")
#mydb = myclient["monitor"]
#mycol = mydb["metrics"]

#postgres connection
pg_file = open("./postgres.secret", "r")
pg_secret_user = pg_file.readline().replace('\n','')
pg_secret_pass = pg_file.readline().replace('\n','')
pg_conn = psycopg2.connect("dbname=monitor user={0} host=localhost password={1}".format(pg_secret_user, pg_secret_pass))
pg_cur = pg_conn.cursor()

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

#tempDict = {
#    "timestamp" : datetime.now().strftime("%Y-%m-%d %H:%M:%S"),
#    "tempValue" : temperature,
#    "tempZoneValue" : ow_temp,
#    "humiValue" : humidity,
#    "humiZoneValue" : ow_humi
#}
#mycol.insert_one(tempDict)

#emoncms.org
emon_key = '16daf72377c2ee9599082b628b76fd45'
emon_json = 'https://emoncms.org/input/post?node=pi4&fulljson={{"temperature":{0},"humidity":{1}}}&apikey={2}'.format(temperature, humidity, emon_key)
requests.get(emon_json)

pg_cur.execute('insert into metrics ("timeStamp", "tempValue", "tempZoneValue", "humiValue", "humiZoneValue") values (%s, %s, %s, %s, %s)', (datetime.now().strftime("%Y-%m-%d %H:%M:%S"), temperature, ow_temp, humidity, ow_humi))
pg_conn.commit()
pg_cur.close()
pg_conn.close()

info_led.off()

dhtDevice.exit()
