using System;
using System.Collections.Generic;
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
            IEnumerable<Transaction> transactions = this._nodeService.GetTransactions();
            IEnumerable<TransactionResource> transactionResources =
                this._mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionResource>>(transactions);
            return Ok(transactions);
        }

        [HttpPost]
        public IActionResult AddTransaction([FromBody] TransactionResource receivedTransaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

//            if (!this._nodeService.CheckSenderBalance(receivedTransaction.From, receivedTransaction.Value))
//            {
//                return BadRequest("Insufficient funds.");
//            }

            Transaction transaction = new Transaction(new Address(receivedTransaction.From),
                new Address(receivedTransaction.To), receivedTransaction.Value, receivedTransaction.Fee,
                receivedTransaction.SenderPublicKey, receivedTransaction.SenderSignature);
            transaction.DateCreated = receivedTransaction.DateCreated;

            transaction.TransactionHash = this._transactionService.CalculateTransactionHash(transaction);

            if (!this._transactionService.Validate(transaction))
            {
                return BadRequest("Invalid transaction signature.");
            }

            if (this._nodeService.CheckForCollison(transaction.TransactionHash))
            {
                return BadRequest("Transaction already exists.");
            }

            this._nodeService.AddTransaction(transaction);
            TransactionResource resource = this._mapper.Map<Transaction, TransactionResource>(transaction);

            return Ok(resource);
        }
    }
}