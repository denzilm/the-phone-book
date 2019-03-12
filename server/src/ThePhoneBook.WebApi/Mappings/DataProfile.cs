using AutoMapper;
using ThePhoneBook.Core.Entities;
using ThePhoneBook.Persistence.Identity;
using ThePhoneBook.WebApi.Api.PhoneBookEntries.Dtos;
using ThePhoneBook.WebApi.Api.PhoneBooks.Dtos;
using ThePhoneBook.WebApi.Api.Users.Dtos;

namespace ThePhoneBook.WebApi.Mappings
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<PhoneBook, PhoneBookResponse>();
            CreateMap<PhoneBook, PhoneBookWithEntriesResponse>();
            CreateMap<PhoneBookEntry, PhoneBookEntryResponse>().ReverseMap();
            CreateMap<PhoneBookEntry, PhoneBookEntryWithPhoneBookResponse>();
            CreateMap<PhoneBookEntryUpdateRequest, PhoneBookEntry>();
            CreateMap<PhoneBookEntryCreateRequest, PhoneBookEntry>();
            CreateMap<PhoneBookUpdateRequest, PhoneBook>();
            CreateMap<PhoneBookCreateRequest, PhoneBook>();
            CreateMap<PhoneBookWithEntriesCreateRequest, PhoneBook>();

            CreateMap<User, RegisterResponse>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress));

            CreateMap<ApplicationIdentityUser, User>()
                .ForMember(dest => dest.IdentityId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}