using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Node.Interfaces;
using Node.Models;
using Node.Resources;

namespace Node.Controllers
{
    [Route("/api/addresses")]
    public class AddressController : Controller
    {
        // GET
        private readonly IMapper _mapper;
        private readonly INodeService _nodeService;

        public AddressController(IMapper mapper, INodeService nodeService)
        {
            this._mapper = mapper;
            this._nodeService = nodeService;
        }

        [HttpGet("{id}")]
        public IActionResult GetAddress(string id)
        {
            var address = this._nodeService.GetAddress(id);

            if (address == null)
            {
                Address newAddress = new Address(id);
                this._nodeService.AddAddress(newAddress);
            }

            var addressResource = this._mapper.Map<Address, AddressResource>(address);
            addressResource.Transactions = this._nodeService.GetTransactionsByAddressId(id);

            return Ok(addressResource);
        }

        [HttpGet("{id}")]
        [Route("/api/addresses/{id}/transactions")]
        public IActionResult GetTransactionsHashesOfAddres(string id)
        {
            IEnumerable<string> result = this._nodeService.GetTransactionsByAddressId(id);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllAddresses()
        {
            var addresses = this._nodeService.GetAllAddresses();

            var addressesResource = this._mapper.Map<IEnumerable<Address>, IEnumerable<AddressResource>>(addresses);

            return Ok(addressesResource);
        }
    }
}