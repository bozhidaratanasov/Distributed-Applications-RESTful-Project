using AutoMapper;
using Data.Entities;
using Repositories.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FragranceAPI.AutoMapper
{
    public class AutoMappings : Profile
    {
        public AutoMappings()
        {
            CreateMap<Fragrance, FragranceDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Sale, SaleDTO>().ReverseMap();
            CreateMap<Sale, SaleCreateDTO>().ReverseMap();
            CreateMap<Sale, SaleUpdateDTO>().ReverseMap();
        }
    }
}
