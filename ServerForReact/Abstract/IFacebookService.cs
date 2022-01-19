using ServerForReact.Models.FacebookResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Abstract
{
    public interface IFacebookService
    {
        public Task<FacebookUserResource> GetUserFromFacebookAsync(string facebookToken);
        public Task<string> CreateUserFromFacebook(FacebookRegisterResource model);
    }
}
