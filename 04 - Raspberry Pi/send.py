import httplib
import json

http = httplib.HTTP()
url = "domotica-nmct.azure-devices.net"

headers = {
    'Accept': 'application/json',
    'Content-Type': 'application/json;charset=UFT-8'
}

body = {

}

response, content = http.request(url, 'POST', json.dumps(body), headers)

print(response)

data = json.loads(content)

print(data)
print(data[0],['ip'])

