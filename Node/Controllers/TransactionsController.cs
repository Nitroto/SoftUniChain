using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Node.Interfaces;
using Node.Models;
using Node.Resources;

namespace Node.Controllers
{
    [Route("/api/transactions")]
    public class TransactionsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly INodeService _nodeService;

        public TransactionsController(IMapper mapper, INodeService nodeService)
        {
            this._mapper = mapper;
            this._nodeService = nodeService;
        }

        [HttpGet("{hash}")]
        public IActionResult GetTransaction(string hash)
        {
            var transaction = this._nodeService.GetTransactionInfo(hash);

            if (transaction == null)
            {
                return NotFound($"Transaction with hash '{hash}' not found.");
            }

            var transactionResource = this._mapper.Map<Transaction, TransactionResource>(transaction);

            return Ok(transactionResource);
        }

        [HttpGet]
        public IActionResult GetAllTransactions()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult AddTransaction([FromBody]TransactionResource transactionResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Transaction transaction = new Transaction(new Address(transactionResource.From),
                new Address(transactionResource.To), transactionResource.Amount);

            if (this._nodeService.GetTransactionsByAddressId(transaction.From.AddressId) != null)
            {
                transaction.Nonce = this._nodeService.GetTransactionsByAddressId(transaction.From.AddressId).Count() + 1;
            }
            else
            {
                transaction.Nonce = 1;
            }
            
            this._nodeService.AddTransaction(transaction);
            
            var result = this._mapper.Map<Transaction, TransactionResource>(transaction);
            return Ok(result);
        }
    }
}