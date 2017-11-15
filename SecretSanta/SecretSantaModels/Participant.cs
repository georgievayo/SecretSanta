using System;
using SecretSantaModels.Enumerations;

namespace SecretSantaModels
{
    public class Participant
    {
        public Guid UserId { get; set; }

        public Guid GroupId { get; set; }

        public ParticipantRole Role { get; set; }
    }
}
