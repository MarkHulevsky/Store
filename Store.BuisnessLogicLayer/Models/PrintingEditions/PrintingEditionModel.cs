using Store.BuisnessLogic.Models.Authors;
using Store.BuisnessLogic.Models.Base;
using System.Collections.Generic;
using static Shared.Enums.Enums;

namespace Store.BuisnessLogic.Models.PrintingEditions
{
    public class PrintingEditionModel : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public CurrencyType Currency { get; set; }
        public PrintingEditionType Type { get; set; }
        public List<AuthorModel> Authors { get; set; }
        public PrintingEditionModel()
        {
            Authors = new List<AuthorModel>();
        }
    }
}
