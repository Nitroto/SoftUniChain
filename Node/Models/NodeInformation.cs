namespace Node.Models
{
    public class NodeInformation
    {
        public string About { get; set; }
        public string Name { get; set; }
        public int Peers { get; set; }
        public int Blocks { get; set; }
        public long ConfirmedTransactions { get; set; }
        public long PendingTransactions { get; set; }
        public int Addresses { get; set; }
        public long Coins { get; set; }
    }
}