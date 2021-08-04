# examples/things.py

# Let's get this party started!
from wsgiref.simple_server import make_server

import falcon
import os
import json

# Falcon follows the REST architectural style, meaning (among
# other things) that you think in terms of resources and state
# transitions, which map to HTTP verbs.
class DevicesResource:
    def on_post(self, req, resp):
        """Handles GET requests"""
        form = req.get_media()
        shell_command = '/bin/bash -c "gatttool -b ' + form["address"] + ' --char-write-req -a ' + form["handle"] + ' -n ' + form["color"] + '"'
        print(shell_command)
        os.system(shell_command)

        resp.status = falcon.HTTP_200
        resp.content_type = falcon.MEDIA_JSON
        resp.text = '{ "response":"ok" }'

# falcon.App instances are callable WSGI apps
# in larger applications the app is created in a separate file
app = falcon.App(middleware=falcon.CORSMiddleware(allow_origins='*', allow_credentials='*'))

# Resources are represented by long-lived class instances
devices = DevicesResource()

# things will handle all requests to the '/things' URL path
app.add_route('/devices/setColor', devices)

if __name__ == '__main__':
    with make_server('', 8000, app) as httpd:
        print('Serving on port 8000...')

        # Serve until process is killed
        httpd.serve_forever()

