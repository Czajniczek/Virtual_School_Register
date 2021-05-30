using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.EmailConfig
{
    public interface IEmailSender
    {
        void SendEmail(MyMessage myMessage);
    }
}
