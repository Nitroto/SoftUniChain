using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Node.Interfaces;
using Node.Models;
using Node.Resources;

namespace Node.Controllers
{
    [Route("/api/info")]
    public class InfoController : Controller
    {
        private INodeService _nodeService;
        private IMapper _mapper;

        public InfoController(INodeService nodeService, IMapper mapper)
        {
            this._nodeService = nodeService;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetInformation()
        {
            var info = this._nodeService.GetNodeInfo();
            var infoResource = this._mapper.Map<NodeInformation, NodeInformationResource>(info);

            return Ok(infoResource);
        }
    }
}