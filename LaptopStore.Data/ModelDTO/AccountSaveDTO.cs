using System;
using System.Collections.Generic;

namespace LaptopStore.Data.ModelDTO
{
    public class AccountSaveDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? Gender { get; set; }
        public string? Address { get; set; }
        public int AccountType { get; set; }
    }
}
