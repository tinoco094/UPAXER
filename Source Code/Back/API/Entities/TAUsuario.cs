using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class TAUsuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apaterno { get; set; }
        public string Amaterno { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public bool Estado { get; set; }

    }
}
