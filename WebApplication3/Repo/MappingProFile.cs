using AutoMapper;
using WebApplication3.Models;
using WebApplication3.Models.DTO;

namespace WebApplication3.Repo
{
    public class MappingProFile : Profile
    {
        public MappingProFile() 
        {
            CreateMap<Product, ProductDTO>(MemberList.Destination).ReverseMap();
            CreateMap<Category, CategoryDTO>(MemberList.Destination).ReverseMap();
            CreateMap<Storage, StorageDTO>(MemberList.Destination).ReverseMap();
        }

    }
}
