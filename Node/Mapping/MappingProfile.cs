using System;
using System.Reflection.Metadata;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Node.Models;
using Node.Resources;

namespace Node.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to API Resource
            CreateMap<Block, BlockResource>();
            CreateMap<Address, AddressResource>();
            CreateMap<Transaction, TransactionResource>();
            CreateMap<NodeInformation, NodeInformationResource>();

            // API Resource to Domain
            CreateMap<BlockResource, Block>();
            CreateMap<AddressResource, Address>();
            CreateMap<TransactionResource, Transaction>();
            CreateMap<NodeInformationResource, NodeInformation>();
        }
    }
}