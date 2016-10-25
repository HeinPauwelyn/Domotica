import serial

ser = serial.Serial('/dev/ttyUSB0', 9600)

while 1:
    text = ser.readline()
    print text