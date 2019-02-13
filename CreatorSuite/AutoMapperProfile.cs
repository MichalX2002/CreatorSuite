using AutoMapper;

namespace CreatorSuite
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDataModel>();
            CreateMap<UserDataModel, User>();
        }
    }
}