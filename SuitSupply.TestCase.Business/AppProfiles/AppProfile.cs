using AutoMapper;
using SuitSupply.TestCase.Data.DTOs;
using SuitSupply.TestCase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuitSupply.TestCase.Business.AppProfiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<ProductDTO, Product>().ReverseMap();
        }
    }
}
