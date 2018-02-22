using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Node.Interfaces;
using Node.Models;
using Node.Resources;

namespace Node.Controllers
{
    [Route("/api/blocks")]
    public class BlocksController : Controller
    {
        private readonly IMapper _mapper;
        private readonly INodeService _nodeService;
        
        public BlocksController(IMapper mapper, INodeService nodeService)
        {
            this._mapper = mapper;
            this._nodeService = nodeService;
        }
        
        [HttpGet("{index}")]
        public IActionResult GetBlock(int index)
        {
            Block block = this._nodeService.GetBlock(index);
            
            if (block == null)
            {
                return NotFound($"Block with index '{index}' not found.");
            }

            BlockResource blockResource = this._mapper.Map<Block, BlockResource>(block);
            
            return Ok(blockResource);
        }
        
        [HttpGet]
        public IActionResult GetAllBlocks()
        {
            IEnumerable<Block> blocks = this._nodeService.GetAllBlocks();

            IEnumerable<BlockResource> blocksResource = this._mapper.Map<IEnumerable<Block>, IEnumerable<BlockResource>>(blocks);
            
            return Ok(blocksResource);
        }
    }
}