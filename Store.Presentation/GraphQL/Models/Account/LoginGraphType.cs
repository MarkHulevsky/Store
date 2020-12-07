﻿using GraphQL.Types;
using Store.BuisnessLogic.Models.Account;

namespace Store.Presentation.GraphQL.Models.Account
{
    public class LoginGraphType : ObjectGraphType<LoginModel>
    {
        public LoginGraphType()
        {
            Field(x => x.Email);
            Field(x => x.Password);
        }
    }
}
