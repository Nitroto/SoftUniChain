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
            CreateMap<Transaction, TransactionResource>();
            CreateMap<NodeInformation, NodeInformationResource>();

            // API Resource to Domain
            CreateMap<BlockResource, Block>();
            CreateMap<TransactionResource, Transaction>();
            CreateMap<NodeInformationResource, NodeInformation>();
        }
    }
}