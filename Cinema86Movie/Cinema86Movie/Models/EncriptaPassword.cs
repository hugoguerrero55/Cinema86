using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Cinema86Movie.Models
{
    public static class EncriptaPassword
    {
        public static string EncriptaTexto(string paasWord)
        {
            try
            {
                return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(paasWord)));
            }
            catch (Exception)
            {

                return null;
            }
            
        }
    }
}