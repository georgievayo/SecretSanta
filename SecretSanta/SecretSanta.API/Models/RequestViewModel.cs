using System;

namespace SecretSanta.API.Models
{
    public class RequestViewModel
    {
        public string GroupName { get; set; }

        public DateTime Date { get; set; }

        public string OwnerName { get; set; }
    }
}