class Miner(object):
    job_finished = False

    def __init__(self, host, port, name, address, threads):
        self.worker_name = name
        self.node = host + ':' + port
        self.payment_address = address
        self.parallel_jobs = int(threads)
        self.step = 100000
        self.nonce = 1


def mine(self):
    """Mine mine ..."""
    while not Miner.job_finished:
        current_nonce_range = self.nonce.incrementAndGet()
