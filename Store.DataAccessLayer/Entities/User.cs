using Microsoft.AspNetCore.Identity;
using Store.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.DataAccessLayer.Entities
{
    public class User: IdentityUser<Guid>, IBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime CreationDate { get; set; }
        public UserStatus Status { get; set; }
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public ICollection<string> Errors { get; set; } = new List<string>();
    }
}
