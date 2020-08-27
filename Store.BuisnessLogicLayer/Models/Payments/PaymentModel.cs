﻿using Store.BuisnessLogic.Models.Base;
using System;

namespace Store.BuisnessLogic.Models.Payments
{
    public class PaymentModel : BaseModel
    {
        public Guid OrderId { get; set; }
        public string UserEmail { get; set; }
        public int Amount { get; set; }
        public string TokenId { get; set; }
        public string CurrencyString { get; set; }
    }
}
