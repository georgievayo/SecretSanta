namespace SecretSanta.API.Models
{
    public class ViewCriteria
    {
        public int Skip { get; set; }

        public int Take { get; set; }

        public string Order { get; set; }

        public string Search { get; set; }
    }
}