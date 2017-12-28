using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Models
{
   public class UserSession
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        [MaxLength(1024)]
        public string AuthToken { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
