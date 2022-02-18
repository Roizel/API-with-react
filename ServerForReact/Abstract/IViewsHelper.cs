using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Abstract
{
    public interface IViewsHelper
    {
        Task<string> GetViewToHtmlAsync<T>(string viewName, T model);
    }
}
