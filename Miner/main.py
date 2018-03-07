import argparse
import sys
from multiprocessing import Process

import time

from miner import Miner

settings = {}


def check_arg(args=None):
    parser = argparse.ArgumentParser()
    parser.add_argument('-H', '--host',
                        help='node ip to connect',
                        default='127.0.0.1')
    parser.add_argument('-p', '--port',
                        help='port to listen on',
                        default=5000)
    parser.add_argument('-n', '--name',
                        help='name of worker',
                        default='worker')
    parser.add_argument('-a', '--address',
                        help='payment address',
                        default='bee3f694bf0fbf9556273e85d43f2e521d24835e')
    parser.add_argument('-t', '--threads',
                        help='parallel threads',
                        default=1)

    result = parser.parse_args(args)
    return result.host, \
           result.port, \
           result.name, \
           result.address, \
           result.threads


def miner_thread(id):
    miner = Miner(settings['host'], settings['port'], settings['payment_address'], id)
    miner.work()


if __name__ == '__main__':
    h, p, n, a, t = check_arg(sys.argv[1:])
    settings['host'] = h
    settings['port'] = int(p)
    settings['threads'] = int(t)
    settings['payment_address'] = a
    settings['name'] = n

    thr_list = []
    for thr_id in range(settings['threads']):
        process = Process(target=miner_thread, args=(settings['name'] + '_' + str(thr_id),))
        process.start()
        thr_list.append(process)
        time.sleep(1)

    print(settings['threads'], "mining threads started")

    print(time.asctime(), "Miner Starts - %s:%s" % (settings['host'], settings['port']))
    try:
        for thr_proc in thr_list:
            thr_proc.join()
    except KeyboardInterrupt:
        pass

print(time.asctime(), "Miner Stops - %s:%s" % (settings['host'], settings['port']))