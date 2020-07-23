using Store.BuisnessLogicLayer.Models.Base;
using Store.BuisnessLogicLayer.Models.PrintingEditions;
using System.Collections.Generic;

namespace Store.BuisnessLogicLayer.Models.Authors
{
    public class AuthorModel: BaseModel
    {
        public string Name { get; set; }
        public List<PrintingEditionModel> PrintingEditions = new List<PrintingEditionModel>();
    }
}
