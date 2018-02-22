namespace Node.Resources
{
    public class NodeInformationResource
    {
        public string  Name { get; set; }
        public string About { get; set; }
        public int Peers { get; set; }
        public int Blocks { get; set; }
        public long ConfirmedTransactions { get; set; }
        public long PendingTransactions { get; set; }
        public int CommulativeDificulty { get; set; }
        public int Addresses { get; set; }
        public long Coins { get; set; }
    }
}