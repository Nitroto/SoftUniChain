import requests
import json

class rpc_client:
    headers = {'content-type': 'application/json'}

    def __init__(self, url):
        self.url = url

    def rpc(self, method, params=None):
        payload = {}
        if params is not None:
            payload = params

        response = None
        if method == 'get_work':
            response = requests.get(self.url, headers=self.headers, timeout=30)
        else:
            requests.post(self.url, data=json.dumps(payload), headers=self.headers, timeout=30)

        if response is None:
            print("JSON-RPC: not response")
            return None

        resp_obj = response.json()
        if resp_obj is None:
            print("JSON-RPC: cannot JSON-decode body")
            return None
        if 'error' in resp_obj and resp_obj['error'] is not None:
            return resp_obj['error']

        return resp_obj

    def submit_result(self, result):
        return self.rpc('submit', result)

    def get_work(self):
        return self.rpc('get_work')
