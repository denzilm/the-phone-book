using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using ThePhoneBook.Core.Entities;
using ThePhoneBook.Persistence.Application;
using ThePhoneBook.Persistence.Identity;

namespace ThePhoneBook.WebApi
{
    public static class SeedData
    {
        public static void Initialise(IServiceProvider serviceProvider)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                ApplicationIdentityDbContext applicationIdentityDbContext =
                    scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
                ApplicationDbContext applicationDbContext =
                    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                applicationDbContext.Database.Migrate();
                applicationIdentityDbContext.Database.Migrate();

                UserManager<ApplicationIdentityUser> userManager =
                    scope.ServiceProvider
                    .GetRequiredService<UserManager<ApplicationIdentityUser>>();

                ApplicationIdentityUser identityUser = userManager
                    .FindByEmailAsync("jane@example.com")
                    .GetAwaiter()
                    .GetResult();

                if (identityUser == null)
                {
                    identityUser = new ApplicationIdentityUser
                    {
                        Email = "jane@example.com",
                        UserName = "jane@example.com",
                        FirstName = "Jane",
                        LastName = "Doe"
                    };

                    IdentityResult result = userManager
                        .CreateAsync(identityUser, "P@ssw0rd")
                        .GetAwaiter()
                        .GetResult();

                    if (result.Succeeded)
                    {
                        IMapper mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                        User user = mapper.Map<User>(identityUser);
                        user.Id = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf");
                        user.PhoneBooks = new List<PhoneBook>
                        {
                            new PhoneBook
                            {
                                Id = new Guid("c7ba6add-09c4-45f8-8dd0-eaca221e5d93"),
                                Title = "The Family",
                                Description = "This phone book contains the contact details for my family",
                                PhoneBookEntries = new List<PhoneBookEntry>
                                {
                                    new PhoneBookEntry
                                    {
                                        Id = new Guid("a3749477-f823-4124-aa4a-fc9ad5e79cd6"),
                                        FirstName = "Jack",
                                        LastName = "Tenrec",
                                        PhoneNumber = "+27219038278",
                                        EmailAddress = "jack@example.com",
                                        Address = "8 Zebra Close, Kuils River"
                                    },
                                    new PhoneBookEntry
                                    {
                                        Id = new Guid("70a1f9b9-0a37-4c1a-99b1-c7709fc64167"),
                                        FirstName = "Hannah",
                                        LastName = "Dundee",
                                        PhoneNumber = "+27219038279",
                                        EmailAddress = "hannah@example.com",
                                        Address = "13 Skyvue Drive, Kuils River"
                                    },
                                    new PhoneBookEntry
                                    {
                                        Id = new Guid("60188a2b-2784-4fc4-8df8-8919ff838b0b"),
                                        FirstName = "Mess",
                                        LastName = "O",
                                        PhoneNumber = "+27219038280",
                                        EmailAddress = "mess@example.com",
                                        Address = "221 Main Road, West Bank"
                                    },
                                    new PhoneBookEntry
                                    {
                                        Id = new Guid("76053df4-6687-4353-8937-b45556748abe"),
                                        FirstName = "Mustapha",
                                        LastName = "P",
                                        PhoneNumber = "+27219038281",
                                        EmailAddress = "mustapha@example.com",
                                        Address = "54 Robert Street, Blackheath"
                                    },
                                    new PhoneBookEntry
                                    {
                                        Id = new Guid("447eb762-95e9-4c31-95e1-b20053fbe215"),
                                        FirstName = "Ginzo",
                                        LastName = "Ninja",
                                        PhoneNumber = "+27219038282",
                                        EmailAddress = "ginzo@example.com",
                                        Address = "19 Fernkloof Crescent, West Bank"
                                    }
                                }
                            }
                        };
                        applicationDbContext.Users.Add(user);
                        applicationDbContext.SaveChanges();
                    }
                }
            }
        }
    }
}