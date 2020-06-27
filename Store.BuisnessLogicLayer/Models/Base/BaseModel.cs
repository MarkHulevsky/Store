using System;
using System.Collections.Generic;

namespace Store.BuisnessLogicLayer.Models.Base
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public ICollection<string> Errors { get; set; } = new List<string>();
    }
}
