import argparse
import sys

from miner import Miner


def check_arg(args=None):
    parser = argparse.ArgumentParser()
    parser.add_argument('-H', '--host',
                        help='node ip to connect',
                        default='127.0.0.1')
    parser.add_argument('-p', '--port',
                        help='port to listen on',
                        default=5050)
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


h, p, n, a, t = check_arg(sys.argv[1:])

worke = Miner(h, p, n, a, t)
