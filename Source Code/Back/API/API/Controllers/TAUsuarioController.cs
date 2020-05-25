using API.Utilities;
using Entities;
using ManejadorBD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Http;

namespace API.Controllers
{
    public class TAUsuarioController : ApiController
    {
        [HttpGet]
        [ActionName("ObtenerModelo")]
        public TAUsuario ObtenerModelo()
        {
            return new TAUsuario();
        }

        [HttpPost]
        [ActionName("Login")]
        public Register Login(Login _login)
        {
            ManejaBD _manejadorBD = new ManejaBD(ConnectionBD.BDUPAXER, eProveedor.SQLSERVER);
            _manejadorBD.AgregaParametro("@Nombre", _login.Username);
            _manejadorBD.AgregaParametro("@Contraseña", Encriptacion.Encriptar(_login.Password));
            DataSet _dt = _manejadorBD.EjecutaStoredProcedure("PRRLogin");

            TAUsuario _reg = ConvertBD.ToObject<TAUsuario>(_dt.Tables[0]);
            return new Register()
            {
                Id = _reg.Id,
                Name = _reg.Nombre,
                Username = _reg.NombreUsuario,
                Email = _reg.Correo,
                LastName = _reg.Apaterno,
                MotherLastName = _reg.Amaterno,
                Password = _reg.Contraseña,
                Estado = _reg.Estado
            };
        }

        [HttpPost]
        [ActionName("Register")]
        public bool Register(Register _register)
        {
            ManejaBD _manejadorBD = new ManejaBD(ConnectionBD.BDUPAXER, eProveedor.SQLSERVER);
            _manejadorBD.AgregaParametro("@Nombre", _register.Name);
            _manejadorBD.AgregaParametro("@Apaterno", _register.LastName);
            _manejadorBD.AgregaParametro("@Amaterno", _register.MotherLastName);
            _manejadorBD.AgregaParametro("@NombreUsuario", _register.Username);
            _manejadorBD.AgregaParametro("@Correo", _register.Email);
            _manejadorBD.AgregaParametro("@Contraseña", Encriptacion.Encriptar(_register.Password));
            DataSet _dt = _manejadorBD.EjecutaStoredProcedure("PRCTAUser");

            if (_dt != null && _dt.Tables.Count == 0 )
            {
                    return true;
            }
            return false;
        }

        [HttpPost]
        [ActionName("Recover")]
        public bool Recover(Recover _recover)
        {
            ManejaBD _manejadorBD = new ManejaBD(ConnectionBD.BDUPAXER, eProveedor.SQLSERVER);
            _manejadorBD.AgregaParametro("@Nombre", _recover.Username);
            DataSet _dt = _manejadorBD.EjecutaStoredProcedure("PRRRecover");

            TAUsuario _reg = ConvertBD.ToObject<TAUsuario>(_dt.Tables[0]);

            if (_reg.Correo != null)
            {
                var _guid = Guid.NewGuid();
                MailMessage mmsg = new MailMessage();

                mmsg.To.Add(_reg.Correo);

                mmsg.Subject = "Recover Password";
                mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

                mmsg.Body = _guid.ToString().Substring(0, 5);
                mmsg.BodyEncoding = System.Text.Encoding.UTF8;
                mmsg.IsBodyHtml = false; //Si no queremos que se envíe como HTML
                mmsg.From = new MailAddress("a@a.com");

                SmtpClient cliente = new SmtpClient();
                // Gmail
                /*
                cliente.Port = 587;
                cliente.EnableSsl = true;
                //cliente.Host = "mail.servidordominio.com";
                //cliente.Credentials = new System.Net.NetworkCredential("c.tinoco@outlook.es", "cruzazul94");
                */
                //Outlook
                cliente.Host = "a@a.com";
                //cliente.Port = 25;
                cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
                cliente.UseDefaultCredentials = false;
                //cliente.EnableSsl = true;
                cliente.Credentials = new NetworkCredential("CorreoEmisor", "CorreoEmisorContraseña");

                return true;
            }
            else
                return false;
        }

        [HttpPost]
        [ActionName("Delete")]
        public bool Delete(Register _register)
        {
            ManejaBD _manejadorBD = new ManejaBD(ConnectionBD.BDUPAXER, eProveedor.SQLSERVER);
            _manejadorBD.AgregaParametro("@Id", _register.Id);
            DataSet _dt = _manejadorBD.EjecutaStoredProcedure("PRDTAUser");

            if (_dt != null && _dt.Tables.Count == 0)
            {
                return true;
            }
            return false;
        }
        [HttpPost]
        [ActionName("Update")]
        public bool Update(Register _register)
        {
            ManejaBD _manejadorBD = new ManejaBD(ConnectionBD.BDUPAXER, eProveedor.SQLSERVER);
            _manejadorBD.AgregaParametro("@Id", _register.Id);
            _manejadorBD.AgregaParametro("@Nombre", _register.Name);
            _manejadorBD.AgregaParametro("@Apaterno", _register.LastName);
            _manejadorBD.AgregaParametro("@Amaterno", _register.MotherLastName);
            _manejadorBD.AgregaParametro("@NombreUsuario", _register.Username);
            _manejadorBD.AgregaParametro("@Correo", _register.Email);
            _manejadorBD.AgregaParametro("@Contraseña", _register.Password);
            DataSet _dt = _manejadorBD.EjecutaStoredProcedure("PRUTAUser");

            if (_dt != null && _dt.Tables.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}