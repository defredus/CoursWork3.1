using DAL.Models;
using BLL.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Profiles
{
    internal class ServicesProfile:Profile
    {
        public ServicesProfile()
        {
            CreateMap<Service, ServicesDTO>().ReverseMap();
        }
    }
}
