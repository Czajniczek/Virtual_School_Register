using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.Helpers
{
    public static class Consts
    {
        public const string PASSWORD_REGEX = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
    }
}
