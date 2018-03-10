using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Node.Interfaces;
using Node.Models;
using Node.Resources;

namespace Node.Controllers
{
    [Produces("application/json")]
    [Route("/api/address")]
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

        [HttpGet]
        public IActionResult GetAllAddresses()
        {
            var addresses = this._nodeService.GetAllAddresses();

            var addressesResource = this._mapper.Map<IEnumerable<Address>, IEnumerable<AddressResource>>(addresses);

            return Ok(addressesResource);
        }

        [HttpGet("{address}")]
        public IActionResult GetAddress(string address)
        {
            var addr = this._nodeService.GetAddress(address);

            if (addr == null)
            {
                Address newAddress = new Address(address);
                this._nodeService.AddAddress(newAddress);
            }

            var addressResource = this._mapper.Map<Address, AddressResource>(addr);

            return Ok(addressResource);
        }

        [HttpGet("{address}/transactions")]
        public IActionResult GetAddressTransactions(string address)
        {
            var addr = this._nodeService.GetAddress(address);

            if (addr == null)
            {
                Address newAddress = new Address(address);
                this._nodeService.AddAddress(newAddress);
            }

            IEnumerable<Transaction> transactions = this._nodeService.GetTransactionsByAddressId(address);
            IEnumerable<TransactionResource> transactionsResources =
                this._mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionResource>>(transactions);

            return Ok(transactionsResources);
        }
    }
}