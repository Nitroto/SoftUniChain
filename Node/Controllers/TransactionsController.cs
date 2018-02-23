using System;
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
        private readonly ITransactionService _transactionService;

        public TransactionsController(IMapper mapper, INodeService nodeService, ITransactionService transactionService)
        {
            this._mapper = mapper;
            this._nodeService = nodeService;
            this._transactionService = transactionService;
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

            Transaction transaction = new Transaction
            {
                From = new Address(transactionResource.From),
                To = new Address(transactionResource.To),
                Value = transactionResource.Value,
                Fee = transactionResource.Fee,
                SenderPublicKey = transactionResource.SenderPublicKey,
                DateCreated = transactionResource.DateCreated,
                TransactionHash = transactionResource.TransactionHash
            };
//            Transaction transaction = this._mapper.Map<TransactionResource, Transaction>(transactionResource);

            bool validTransaction = this._transactionService.Validate(transaction);

            if (!validTransaction)
            {
                return BadRequest("Invalid transaction.");
            }

            this._nodeService.AddTransaction(transaction);

            return Ok(transactionResource);
        }
    }
}