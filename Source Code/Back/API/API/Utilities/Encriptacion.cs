using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace API.Utilities
{
    public static class Encriptacion
    {
        private static string password = "wisloop";
        //public static string encriptacion(string _texto)
        //{
        //    byte[] _arrayTexto = Encoding.ASCII.GetBytes(_texto);
        //    SHA256 _SHA256 = SHA256.Create();
        //    byte[] _textoEncriptado = _SHA256.ComputeHash(_arrayTexto);
        //    StringBuilder _builder = new StringBuilder();
        //    for (int i = 0; i < _textoEncriptado.Length; i++)
        //    {
        //        _builder.Append(_textoEncriptado[i].ToString("x2"));
        //    }
        //    return _builder.ToString();
        //}
        //public static string desEncriptacion(string _texto)
        //{
        //    byte[] _arrayTexto = Encoding.ASCII.GetBytes(_texto);
        //    SHA256 _SHA256 = SHA256.Create();
        //    byte[] _textoEncriptado = _SHA256.ComputeHash(_arrayTexto);
        //    StringBuilder _builder = new StringBuilder();
        //    for (int i = 0; i < _textoEncriptado.Length; i++)
        //    {
        //        _builder.Append(_textoEncriptado[i].ToString("x2"));
        //    }
        //    return _builder.ToString();
        //}
        public static string Encriptar(string plainText)
        {
            if (plainText == null)
            {
                return null;
            }

            if (password == null)
            {
                password = String.Empty;
            }
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = Encriptacion.Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        public static string DesEncriptar(string encryptedText)
        {
            if (encryptedText == null)
            {
                return null;
            }

            if (password == null)
            {
                password = String.Empty;
            }
            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesDecrypted = Encriptacion.Decrypt(bytesToBeDecrypted, passwordBytes);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }
        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
    }
}