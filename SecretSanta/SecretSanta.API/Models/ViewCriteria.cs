using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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