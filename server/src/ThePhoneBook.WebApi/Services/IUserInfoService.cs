namespace ThePhoneBook.WebApi.Services
{
    public interface IUserInfoService
    {
        string UserId { get; }
        string FirstName { get; }
        string LastName { get; }
    }
}