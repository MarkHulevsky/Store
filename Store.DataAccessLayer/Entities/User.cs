﻿using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;
using Store.DataAccess.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.DataAccess.Entities
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        [Computed]
        public virtual List<Order> Order { get; set; }
        [NotMapped]
        public string Password { get; set; }
        public User()
        {
            IsActive = true;
            Order = new List<Order>();
        }
    }
}
