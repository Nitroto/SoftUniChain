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
//            var blocks = this._nodeService.GetAllBlocks();
//
//            var blocksResource = this._mapper.Map<IEnumerable<Block>, IEnumerable<BlockResource>>(blocks);
//            
//            return Ok(blocksResource);

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
            
            this._nodeService.AddTransaction(transaction);
            
            var result = this._mapper.Map<Transaction, TransactionResource>(transaction);
            return Ok(result);
        }
    }
}