using System;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Models.Base
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<string> Errors { get; set; }
        public BaseModel()
        {
            CreationDate = DateTime.Now;
            Errors = new List<string>();
        }
    }
}
