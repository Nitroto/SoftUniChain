import requests
import json

FAKE_ANSWER = {
    "index": 1,
    "transactions": [
        {
            "from": "dc2991f4349d2e9779961ef4f90a37ed92905632",
            "to": "c119abbd1c06c92087b870b3be833cd3987b2a09",
            "senderPublicKey": "845ee29ff8c207073a5211b2a70eed7af5f8b80614fd4f0215c9bc39f06ea16e0",
            "value": 1000000,
            "fee": 20,
            "dateCreated": "2018-03-06T08:39:15.191Z",
            "senderSignature": [
                "fc68a2647ee170f008d85bf5ac53a57f829053d05e9e8000fd3de00133ec072f",
                "476e5c0a48ee6cb1ecde846ae4a3dde8ad164004018983ed5e1f46d4d53a68f9"
            ],
            "transactionHash": "6a5ac83b6a1d5463573d75540fe9b7e6322af0461bb7414d6747e9c075cc1337",
            "transferSuccessful": True
        }
    ],
    "difficulty": 5,
    "prevBlockHash": "51206e803cdb1f6995c388d24881a1bad11ade412debf5fec0179b5a9ad83fe7",
    "minedBy": "bee3f694bf0fbf9556273e85d43f2e521d24835e"
}


class rpc_client:
    headers = {'content-type': 'application/json'}

    def __init__(self, url):
        self.url = url

    def rpc(self, method, params=None):
        payload = []
        if params is not None:
            payload['params'] = params

        if method == 'get_work':
            response = requests.get(self.url, headers=self.headers, timeout=30)
            # response = FAKE_ANSWER
        else:
            response = requests.post(self.url, data=json.dumps(payload), headers=self.headers, timeout=30)

        if response is None:
            print("JSON-RPC: not response")
            return None

        resp_obj = response.json()
        if resp_obj is None:
            print("JSON-RPC: cannot JSON-decode body")
            return None
        if 'error' in resp_obj and resp_obj['error'] is not None:
            return resp_obj['error']
        if 'content' not in resp_obj:
            print("JSON-RPC: no result in object")
            return None

        return resp_obj['content']

    def submit_result(self, result):
        return self.rpc('get_block_count', result)

    def get_work(self):
        return self.rpc('get_work')
