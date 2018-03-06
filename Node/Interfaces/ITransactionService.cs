using Node.Models;

namespace Node.Interfaces
{
    public interface ITransactionService
    {
        void Create(Transaction transaction);
        bool Validate(Transaction transaction);
        string CalculateTransactionHash(Transaction transaction);
    }
}