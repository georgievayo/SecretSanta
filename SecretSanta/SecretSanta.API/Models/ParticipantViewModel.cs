﻿namespace SecretSanta.API.Models
{
    public class ParticipantViewModel
    {
        public ParticipantViewModel(string username, string displayName, string email)
        {
            Username = username;
            DisplayName = displayName;
            Email = email;
        }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }
    }
}