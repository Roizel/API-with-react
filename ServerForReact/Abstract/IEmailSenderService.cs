using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Abstract
{
    public interface IEmailSenderService
    {
        public Task Day();
        public void Week();
        public void Mounth();
    }
}
