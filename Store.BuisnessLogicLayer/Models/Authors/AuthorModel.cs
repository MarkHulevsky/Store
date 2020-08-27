using Store.BuisnessLogic.Models.Base;
using Store.BuisnessLogic.Models.PrintingEditions;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Models.Authors
{
    public class AuthorModel : BaseModel
    {
        public string Name { get; set; }
        public List<PrintingEditionModel> PrintingEditions { get; set; }
        public AuthorModel()
        {
            PrintingEditions = new List<PrintingEditionModel>();
        }
    }
}
