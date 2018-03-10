using System;
using System.Reflection.PortableExecutable;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Node.Interfaces;
using Node.Models;
using Node.Resources;

namespace Node.Controllers
{
    [Route("/api/mining")]
    public class MiningController : Controller
    {
        private readonly IMapper _mapper;
        private readonly INodeService _nodeService;

        public MiningController(IMapper mapper, INodeService nodeService)
        {
            this._mapper = mapper;
            this._nodeService = nodeService;
        }

        [HttpGet("{address}")]
        public IActionResult GetCandidate(string address)
        {
            Block candidateBlock = this._nodeService.GetBlockCandidate();
            this._nodeService.AddMiningJob(address, candidateBlock);
            candidateBlock.MinedBy = this._nodeService.GetAddress(address).AddressId;
            BlockResource candidateResource = this._mapper.Map<Block, BlockResource>(candidateBlock);
            return Ok(candidateResource);
        }

        [HttpPost("{address}")]
        public IActionResult BlockFound(string address, [FromBody] BlockResource confirmation)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("shit");
                return BadRequest(ModelState);
            }

            Block lastMiningBlock = this._nodeService.GetMiningJob(address);
            lastMiningBlock.MinedBy = address;
            lastMiningBlock.Nonce = confirmation.Nonce;
            lastMiningBlock.BlockHash = confirmation.BlockHash;
            lastMiningBlock.CreatedOn = DateTime.Parse(confirmation.CreatedOn);
            
//            Block confirmedBlock = this._mapper.Map<BlockResource, Block>(confirmedBlockResource);
            if (!this._nodeService.IsBlockValid(lastMiningBlock))
            {
                return BadRequest("Not valid block");
            }

            this._nodeService.PayForBlock(address);

            this._nodeService.UpdateBlockchain(lastMiningBlock);

            return Ok();
        }
    }
}