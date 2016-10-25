import serial

ser = serial.Serial('/dev/ttyUSB0', 9600)
counter = 0

while counter <= 100:
    text = ser.readline()
    print text
    counter += 1