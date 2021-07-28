import bluetooth

# Bluetooth stuff
bd_addr = "14:4F:8A:A8:17:CB"
port = 1
sock = bluetooth.BluetoothSocket( bluetooth.RFCOMM )
sock.connect((bd_addr, port))

# 0x1X for straight forward and 0x11 for very slow to 0x1F for fastest
sock.send('\x1A')

