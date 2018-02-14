using System.Threading.Tasks;
using System.Transactions;

namespace Node.Interfaces
{
    public interface ITransactionService
    {
        void Create(Transaction transaction);
        Transaction Sign(Transaction transaction, string privateKey);
        bool Validate(Transaction transaction);
    }
}