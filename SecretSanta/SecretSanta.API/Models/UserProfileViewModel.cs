namespace SecretSanta.API.Models
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel(string email, string username, int age, 
            string interests, string phoneNumber, string displayName, string address)
        {
            Email = email;
            Username = username;
            Age = age;
            Interests = interests;
            PhoneNumber = phoneNumber;
            DisplayName = displayName;
            Address = address;
        }

        public string Email { get; set; }

        public string Username { get; set; }

        public int Age { get; set; }

        public string Interests { get; set; }

        public string PhoneNumber { get; set; }

        public string DisplayName { get; set; }

        public string Address { get; set; }
    }
}