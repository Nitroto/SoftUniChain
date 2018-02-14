using System.Threading.Tasks;
using System.Transactions;
using Node.Interfaces;

namespace Node.Services
{
    public class TransactionService : ITransactionService
    {
        public void Create(Transaction transaction)
        {
            throw new System.NotImplementedException();
        }

        public Transaction Sign(Transaction transaction, string privateKey)
        {
            // get transactionData
            // hash transactionData
            // sign transaction data
            // populate signature in the transaction
            throw new System.NotImplementedException();
        }

        public bool Validate(Transaction transaction)
        {
            // get transactionData
            // hash transactionData
            // validate signature???

            return true;
        }
    }
}