using System;

namespace ThePhoneBook.Core.Entities
{
    /// <summary>
    /// Base class for all entity class
    /// </summary>
    public class BaseEntity
    {
        public Guid Id { get; set; }
    }
}