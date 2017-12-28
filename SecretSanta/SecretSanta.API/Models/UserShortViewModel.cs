namespace SecretSanta.API.Models
{
    public class UserShortViewModel
    {
        public UserShortViewModel(string username, string displayName, string phoneNumber, string email)
        {
            Username = username;
            DisplayName = displayName;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}