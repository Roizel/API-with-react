using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Exceptions
{
    public class AccountException : Exception
    {
        public AccountError AccountError { get; private set; }
        public AccountException(AccountError accountError)
        {
            AccountError = accountError;
        }
    }
}
