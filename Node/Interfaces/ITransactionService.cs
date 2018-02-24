using Node.Models;

namespace Node.Interfaces
{
    public interface ITransactionService
    {
        void Create(Transaction transaction);
        Transaction Sign(Transaction transaction, string privateKey);
        bool Validate(Transaction transaction);
        string CalculateTransactionHash(Transaction transaction);
    }
}