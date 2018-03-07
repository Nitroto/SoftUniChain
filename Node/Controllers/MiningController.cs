using System;
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
        }

        [HttpGet("{address}")]
        public IActionResult GetCandidate(string address)
        {
            Block candidateBlock = this._nodeService.GetBlockCandidate();
            candidateBlock.MineBy = this._nodeService.GetAddress(address);
            BlockResource candidateResource = this._mapper.Map<Block, BlockResource>(candidateBlock);
            return Ok(candidateResource);
        }

        [HttpPost("{address}")]
        public IActionResult BlockFound(string address)
        {
            return Ok();
        }
    }
}