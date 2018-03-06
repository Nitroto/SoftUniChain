using System.Collections.Generic;
using Node.Interfaces;
using Node.Models;
using SQLitePCL;

namespace Node.Services
{
    public class PeerService : IPeerService
    {
        public IList<Peer> Peers { get; private set; }

        public void SyncPeers()
        {
            throw new System.NotImplementedException();
        }

        public void ClearPeers()
        {
            this.Peers.Clear();
        }

        public void AddNewPeer(Peer peer)
        {
            Peer newPeer = new Peer(peer.Name, peer.Address);
            this.Peers.Add(newPeer);
        }

        public IList<Block> GetBlocks(string nodeAddress)
        {
            throw new System.NotImplementedException();
        }

        public IList<Block> SyncBlocks()
        {
            throw new System.NotImplementedException();
        }
    }
}