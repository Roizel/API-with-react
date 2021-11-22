using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Exceptions
{
    public class AccountException : Exception /*Create castom Exceptions for send errors to frontend*/
    {
        public AccountError AccountError { get; private set; } /*Create exmp of AccountError*/
        public AccountException(AccountError accountError)
        {
            AccountError = accountError;
        }
    }
}
