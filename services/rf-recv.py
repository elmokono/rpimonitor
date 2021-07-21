#!/usr/bin/env python3

import argparse
import signal
import sys
import time
import logging
import board
import adafruit_dht
#import pymongo
import psycopg2
from datetime import datetime
from gpiozero import LED

from rpi_rf import RFDevice
rfdevice = None

signal_led = LED(23)

# pylint: disable=unused-argument
def exithandler(signal, frame):
    rfdevice.cleanup()
    sys.exit(0)

#mongodb connection
#myclient = pymongo.MongoClient("mongodb://127.0.0.1:27017/")
#mydb = myclient["monitor"]
#mycol = mydb["rf"]

#postgres connection
pg_file = open("./postgres.secret", "r")
pg_secret_user = pg_file.readline().replace('\n','')
pg_secret_pass = pg_file.readline().replace('\n','')
pg_conn = psycopg2.connect("dbname=monitor user={0} host=localhost password={1}".format(pg_secret_user, pg_secret_pass))
pg_cur = pg_conn.cursor()

logging.basicConfig(level=logging.INFO, datefmt='%Y-%m-%d %H:%M:%S',
                    format='%(asctime)-15s - [%(levelname)s] %(module)s: %(message)s', )

parser = argparse.ArgumentParser(description='Receives a decimal code via a 433/315MHz GPIO device')
parser.add_argument('-g', dest='gpio', type=int, default=20,
                    help="GPIO pin (Default: 20)")
args = parser.parse_args()

signal.signal(signal.SIGINT, exithandler)
rfdevice = RFDevice(args.gpio)
rfdevice.enable_rx()
timestamp = None
logging.info("Listening for codes on GPIO " + str(args.gpio))
while True:
    if rfdevice.rx_code_timestamp != timestamp:
        signal_led.on()
        timestamp = rfdevice.rx_code_timestamp
        logging.info(str(rfdevice.rx_code) +
                     " [pulselength " + str(rfdevice.rx_pulselength) +
                     ", protocol " + str(rfdevice.rx_proto) + "]")
        #signalDict = {
        # "timestamp" : datetime.now().strftime("%Y-%m-%d %H:%M:%S"),
        # "code" : rfdevice.rx_code,
        # "pulseLength" : rfdevice.rx_pulselength,
        # "protocol" : rfdevice.rx_proto
        #}
        #mycol.insert_one(signalDict)

        pg_cur.execute(
                       'insert into rf ("timeStamp", "code", "pulseLength", "protocol") values (%s, %s, %s, %s)',
                       (datetime.now().strftime("%Y-%m-%d %H:%M:%S"), rfdevice.rx_code, rfdevice.rx_pulselength,rfdevice.rx_proto))
        pg_conn.commit()
        #pg_cur.close()
        #pg_conn.close()

        signal_led.off()
    time.sleep(0.01)
rfdevice.cleanup()

