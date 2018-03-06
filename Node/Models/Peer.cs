namespace Node.Models
{
    public class Peer
    {
        public Peer(string name, string address)
        {
            this.Name = name;
            this.Address = address;
            this.Reputation = 0;
        }

        public string Address { get; set; }
        public string Name { get; set; }
        public int Reputation { get; set; }
    }
}