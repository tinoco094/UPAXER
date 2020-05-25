using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class Register
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string MotherLastName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Estado { get; set; }
    }
    public class Recover : Login
    {

    }
}
