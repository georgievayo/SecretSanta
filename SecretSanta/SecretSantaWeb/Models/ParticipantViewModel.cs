﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSantaWeb.Models
{
    public class ParticipantViewModel
    {
        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string PhotoUrl { get; set; }
    }
}