using Cinema86Movie.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Data.Entity;

namespace ClubCinema86.Controllers
{
    public class LoginController : Controller
    {
        DB_A4B7B0_Cinema86Entities objCon = new DB_A4B7B0_Cinema86Entities();

        // GET: Login
        [OutputCache(Duration = 60, Location =OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Index(Login LgnUsr)
        {
            var _passWord = Cinema86Movie.Models.EncriptaPassword.EncriptaTexto(LgnUsr._Password);
            DateTime _fechaVigenciaFormat = _getServerDateTime();//Convert.ToDateTime(_fechaVigencia);
            bool Isvalid = objCon.CN86_Usuario.Where(x => DbFunctions.TruncateTime(x.Fecha_Expiracion) >= DbFunctions.TruncateTime(_fechaVigenciaFormat))
                           .Any(x => x.eMail == LgnUsr._eMail && x.eMail_Confirmed == true && x.Password == _passWord && x.Id_Estatus == 1);

            if (Isvalid)
            {
                int timeout = LgnUsr.Rememberme ? 60 : 5; // Timeout in minutes, 60 = 1 hour.  
                var ticket = new FormsAuthenticationTicket(LgnUsr._eMail, false, timeout);
                string encrypted = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                cookie.Expires = System.DateTime.Now.AddMinutes(timeout);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);

                return RedirectToAction("Main", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Correo o Contraseña Inválidos, Verifique que su cuenta no haya caducado... Intente de nuevo!");
            }
            return View();
        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }


        public ActionResult OlvidoPassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult OlvidoPassword(OlvidoPassword pass)
        {
            var IsExists = IsEmailExists(pass._eMail);
            if (!IsExists)
            {
                ModelState.AddModelError("EmailNotExists", "Este correo no existe");
                return View();
            }
            var objUsr = objCon.CN86_Usuario.Where(x => x.eMail == pass._eMail).FirstOrDefault();

            // Genrate OTP   
            string OTP = GeneratePassword();

            objUsr.ActivetionCode = Guid.NewGuid().ToString();
            objUsr.OTP = OTP;
            objCon.Entry(objUsr).State = System.Data.Entity.EntityState.Modified;
            objCon.SaveChanges();

            ForgetPasswordEmailToUser(pass._eMail, objUsr.ActivetionCode.ToString(), objUsr.OTP);
            ViewBag.Message = "Se ha enviado un codigo OPT a su cuenta de correo, verifique su cuenta de email por favor, si no encuentra el correo titulado " +
                "<Cinema86 Reset de Contraseña> en su Inbox o bandeja de entrada por favor verifique en correos no deseados posiblemente ahi se localice. Presione Salir.";
            return View("CambioPasswordEnviado");
        }

        public bool IsEmailExists(string eMail)
        {
            var IsCheck = objCon.CN86_Usuario.Where(email => email.eMail == eMail).FirstOrDefault();
            return IsCheck != null;
        }


        public string GeneratePassword()
        {
            string OTPLength = "4";
            string OTP = string.Empty;

            string Chars = string.Empty;
            Chars = "1,2,3,4,5,6,7,8,9,0";

            char[] seplitChar = { ',' };
            string[] arr = Chars.Split(seplitChar);
            string NewOTP = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < Convert.ToInt32(OTPLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                NewOTP += temp;
                OTP = NewOTP;
            }
            return OTP;
        }

        public void ForgetPasswordEmailToUser(string emailId, string activationCode,string OPT)
        {
            try
            {
                string _FromEmailAddress = ConfigurationManager.AppSettings["Sender"].ToString();
                string _FromContresena = ConfigurationManager.AppSettings["PswSender"].ToString();
                string _Smtp = ConfigurationManager.AppSettings["SMTP"].ToString();
                int _Port = int.Parse(ConfigurationManager.AppSettings["PORT"].ToString());

                var GenarateChangePasswordVerificationLink = "/Login/CambiarPassword/" + activationCode;
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenarateChangePasswordVerificationLink);

                var fromMail = new MailAddress(_FromEmailAddress, "Cinema86 Reset de Contraseña"); // set your email  
                var fromEmailpassword = _FromContresena;
                var toEmail = new MailAddress(emailId);

                var smtp = new SmtpClient();
                smtp.Host = _Smtp;
                smtp.Port = _Port;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

                var Message = new MailMessage(fromMail, toEmail);
                Message.Subject = "Resetear Contraseña";
                Message.Body = "<br/> Para resetear su contraseña," +
                               "<br/> haga clic por favor en la liga de abajo." +
                               "<br/> Su clave OPT es:< " + OPT + " > No la pierda por favor sera requerida para completar el proceso de cambio de contraseña." +
                               "<br/><br/><a href=" + link + ">" + link + "</a>";
                Message.IsBodyHtml = true;
                smtp.Send(Message);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult CambiarPassword()
        {
            return View();
        }

        [HttpPost]
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult CambiarPassword(CambiarPassword _ChangePass,string OTP)
        {
            try
            {
                var objUsr = objCon.CN86_Usuario.Where(x => x.OTP == OTP).FirstOrDefault();

                objUsr.Password = Cinema86Movie.Models.EncriptaPassword.EncriptaTexto(_ChangePass.Password); 
                objCon.SaveChanges();

                ViewBag.Message = "Cambio de contraseña se ha realizado con éxito";
                return View("CambioPasswordEnviado");
            }
            catch (Exception)
            {
                ViewBag.Message = "Ocurrio un error al cambiar la contraseña, intentelo de nuevo ingresando a la liga que se envuio por correo";
                return View("CambioPasswordError");
            }

        }

        private DateTime _getServerDateTime()
        {
            try
            {
                DateTime dateTimeServer = objCon.Database.SqlQuery<DateTime>("SELECT GETDATE() Fecha").SingleOrDefault();

                return dateTimeServer;
            }
            catch (Exception)
            {
                return Convert.ToDateTime("1900-01-01 00:00:00.000"); 
            }
        }
    }
}