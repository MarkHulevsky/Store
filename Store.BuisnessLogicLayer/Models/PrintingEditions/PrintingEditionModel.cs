using Store.BuisnessLogicLayer.Models.Authors;
using Store.BuisnessLogicLayer.Models.Base;
using System.Collections.Generic;
using static Store.BuisnessLogicLayer.Models.Enums.Enums;

namespace Store.BuisnessLogicLayer.Models.PrintingEditions
{
    public class PrintingEditionModel: BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public Currency Currency { get; set; }
        public PrintingEditionType Type { get; set; }
        public IEnumerable<AuthorModel> Authors { get; set; } = new List<AuthorModel>();
    }
}
