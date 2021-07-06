#!/usr/bin/env python3

import argparse
import signal
import sys
import time
import logging
import board
import adafruit_dht
import pymongo
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
myclient = pymongo.MongoClient("mongodb://127.0.0.1:27017/")
mydb = myclient["monitor"]
mycol = mydb["rf"]

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
        signalDict = {
         "timestamp" : datetime.now().strftime("%Y-%m-%d %H:%M:%S"),
         "code" : rfdevice.rx_code,
         "pulseLength" : rfdevice.rx_pulselength,
         "protocol" : rfdevice.rx_proto
        }
        mycol.insert_one(signalDict)
        signal_led.off()
    time.sleep(0.01)
rfdevice.cleanup()
