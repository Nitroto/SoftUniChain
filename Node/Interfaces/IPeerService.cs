using System.Collections.Generic;
using Node.Models;

namespace Node.Interfaces
{
    public interface IPeerService
    {
        IList<Peer> Peers { get; }

        void SyncPeers();
        void ClearPeers();
        void AddNewPeer(Peer peer);
        IList<Block> GetBlocks(string nodeAddress);
        IList<Block> SyncBlocks();
    }
}