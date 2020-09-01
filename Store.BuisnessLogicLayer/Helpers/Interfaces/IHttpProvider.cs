﻿using System.Threading.Tasks;

namespace Store.BuisnessLogic.Helpers.Interfaces
{
    public interface IHttpProvider
    {
        Task<string> GetHttpContentAsync(string url);
    }
}
