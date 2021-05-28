using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.ViewModels.UsersController
{
    public class UserIndexViewModel
    {
        public string Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Parent { get; set; }

        public string Type { get; set; }

        public string Class { get; set; }
    }
}
