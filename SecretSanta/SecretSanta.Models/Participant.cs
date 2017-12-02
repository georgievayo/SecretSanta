using System;

namespace SecretSanta.Models
{
    public class Participant
    {
        public Guid UserId { get; set; }

        public Guid GroupId { get; set; }

        public ParticipantRole Role { get; set; }
    }
}
