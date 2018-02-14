using System.Collections.Generic;
using System.Threading.Tasks;
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
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlock(int index)
        {
            var block = this._nodeService.GetBlock(index);
            
            if (block == null)
            {
                return NotFound();
            }

            var blockResource = this._mapper.Map<Block, BlockResource>(block);
            
            return Ok(blockResource);
        }
        
        [HttpGet]
        // Shoold return BlockResource
        public async Task<IActionResult> GetAllBlocks()
        {
            var blocks = this._nodeService.GetAllBlocks();

            var blocksResource = this._mapper.Map<IEnumerable<Block>, IEnumerable<BlockResource>>(blocks);
            
            return Ok(blocksResource);
        }


    }
}