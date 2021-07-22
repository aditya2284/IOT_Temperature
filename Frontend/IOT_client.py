import os
import glob
import time
import calendar
import grpc
import Temperature_pb2
import Temperature_pb2_grpc
from google.protobuf.timestamp_pb2 import Timestamp
from datetime import datetime
import random

os.system('modprobe w1-gpio')
os.system('modprobe w1-therm')
 
base_dir = '/sys/bus/w1/devices/'
device_folder = glob.glob(base_dir + '28*')[0]
device_file = device_folder + '/w1_slave'

def read_temp_raw():
    f = open(device_file, 'r')
    lines = f.readlines()
    f.close()
    return lines
    
def read_temp():
    lines = read_temp_raw()
    while lines[0].strip()[-3:] != 'YES':
        time.sleep(0.2)
        lines = read_temp_raw()
    equals_pos = lines[1].find('t=')
    if equals_pos != -1:
        temp_string = lines[1][equals_pos+2:]
        temp_c = float(temp_string) / 1000.0
        return temp_c

#step1:create a channel
channel=grpc.insecure_channel('192.168.1.13:30051')

#Step2: create a stub
stub=Temperature_pb2_grpc.TemperatureSensingStub(channel)

#Step3: call API

while True:
    now = datetime.now()
    timestamp = Timestamp()
    timestamp.FromDatetime(now)
    deviceID=str(random.randint(1,10))
    tosend=Temperature_pb2.TemperatureData(device_ID=deviceID,date_time=timestamp,temperature=read_temp(),unit="degree C")
    response=stub.AddTemperatureData(tosend)
    time.sleep(10)


