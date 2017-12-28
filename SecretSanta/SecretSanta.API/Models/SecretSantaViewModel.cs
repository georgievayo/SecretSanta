namespace SecretSanta.API.Models
{
    public class SecretSantaViewModel
    {
        public SecretSantaViewModel(string receiver)
        {
            Receiver = receiver;
        }

        public string Receiver { get; set; }
    }
}