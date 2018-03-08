import datetime
import sys
import pprint
import hashlib

from rpc_client import rpc_client

MAX_NONCE = sys.maxsize
pp = pprint.PrettyPrinter(indent=4)


class Miner(object):
    def __init__(self, host, port, payment_address, id):
        self.url = 'http://' + host + ':' + str(port) + '/api/mining/' + payment_address
        self.host = host
        self.port = port
        self.payment_address = payment_address
        self.id = id
        self.max_nonce = MAX_NONCE
        self.nonce = 0
        self.difficulty = ""
        self.target_hash = ""

    def work(self):
        # if not self.check_difficulty():
        #     raise Exception('No proper difficulty')
        # if not self.check_target_hash():
        #     raise Exception('No proper target hash')
        while True:
            self.loop()

    def loop(self):
        rpc = rpc_client(self.url)
        if rpc is None:
            return
        block = rpc.get_work()
        print(block)
        timestamp = datetime.datetime.now().isoformat()

        self.difficulty = '0' * block['difficulty'] + '9' * (64 - block['difficulty'])
        precomputed_data = str(block['index']) + block['blockDataHash'] + block['previousBlockHash']
        block_found = False
        while not block_found and self.nonce < MAX_NONCE:
            data = precomputed_data + timestamp + str(self.nonce)
            block_hash = hashlib.sha256(data.encode('utf-8')).hexdigest()
            if block_hash < self.difficulty:
                print('!!! Block found !!!')
                print('!!! Block hash: ', block_hash)
                result = {
                    'nonce': str(self.nonce),
                    'dateCreated': timestamp,
                    'blockHash': block_hash
                }
                block_found = True

                rpc.submit_result(result)

            if self.nonce % 1000000 == 0:
                print(timestamp)
                print('Nonce: ', self.nonce)
                print('Block Hash: ', block_hash)

            if self.nonce % 100000 == 0:
                timestamp = datetime.datetime.now().isoformat()

            self.nonce += 1

    def check_target_hash(self):
        if len(self.target_hash) <= 0:
            return False

        return True

    def check_difficulty(self):
        if len(self.difficulty) <= 0:
            return False
        if not self.difficulty.isdigit():
            return False
        check = "0" * len(self.difficulty)
        if self.difficulty != check:
            return False
        return True
