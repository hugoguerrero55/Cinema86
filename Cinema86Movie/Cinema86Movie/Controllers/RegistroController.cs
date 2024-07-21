using Cinema86Movie.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.UI;

namespace ClubCinema86.Controllers
{
    public class RegistroController : Controller
    {
        DB_A4B7B0_Cinema86Entities objCon = new DB_A4B7B0_Cinema86Entities();

        public void InicializaControles()
        {
            var listaEstados = new List<CN86_Estado>();

            using (DB_A4B7B0_Cinema86Entities db = new DB_A4B7B0_Cinema86Entities())
            {
                listaEstados = db.CN86_Estado.OrderBy(x => x.Descripcion).ToList();
            }

            ViewBag.ID = new SelectList(listaEstados, "Id_Estado", "Descripcion");
        }

        #region Check Email Exists or not in DB  
        public bool IsEmailExists(string eMail)
        {
            var IsCheck = objCon.CN86_Usuario.Where(email => email.eMail == eMail).FirstOrDefault();
            return IsCheck != null;
        }
        #endregion


        #region Verification from Email Account.  
        public ActionResult UserVerification(string id)
        {
            bool Status = false;

            objCon.Configuration.ValidateOnSaveEnabled = false; // Ignor to password confirmation   
            var IsVerify = objCon.CN86_Usuario.Where(u => u.ActivetionCode == new Guid(id).ToString()).FirstOrDefault();

            if (IsVerify != null)
            {
                IsVerify.eMail_Confirmed = true;
                IsVerify.Id_Estatus = 1; //Cambio el estatus del usuario a activo este es clave para activar los usuarios
                objCon.SaveChanges();
                ViewBag.Message = "Verificación de Correo Completada";
                Status = true;
            }
            else
            {
                ViewBag.Message = "Peticion Invalida...Correo no Verificado";
                ViewBag.Status = false;
            }

            return View();
        }
        #endregion

        public void SendEmailToSistemOperator(string _eMail, string _nombreCompleto, string _fechaReg, string _fechaExpira)
        {
            try
            {
                string _FromEmailAddress = ConfigurationManager.AppSettings["Sender"].ToString();
                string _FromContresena = ConfigurationManager.AppSettings["PswSender"].ToString();
                string _Smtp = ConfigurationManager.AppSettings["SMTP"].ToString();
                string _emailId = _FromEmailAddress = ConfigurationManager.AppSettings["SOReceiver"].ToString();
                int _Port = int.Parse(ConfigurationManager.AppSettings["PORT"].ToString());

                var fromMail = new MailAddress(_FromEmailAddress, "Cinema86 Registro de Usuarios Nuevos"); 
                var fromEmailpassword = _FromContresena;
                var toEmail = new MailAddress(_emailId);

                var smtp = new SmtpClient();
                smtp.Host = _Smtp;
                smtp.Port = _Port;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

                var Message = new MailMessage(fromMail, toEmail);
                Message.Subject = "Un nuevo usuario se ha registrado en la plataforma Cinema 86";
                Message.Body = "<br/> Por favor mantengase atento a esta cuenta para proporcionarle el mejor servicio" +
                               "<br/> Nombre: " + _nombreCompleto +
                               "<br/> Correo: " + _eMail +
                               "<br/> Fecha de Registro: " + _fechaReg +
                               "<br/> Fecha de Expiración de la cuenta: " + _fechaExpira;
                Message.IsBodyHtml = true;
                smtp.Send(Message);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SendEmailToUser(string emailId, string activationCode)
        {
            try
            {
                string _FromEmailAddress = ConfigurationManager.AppSettings["Sender"].ToString();
                string _FromContresena = ConfigurationManager.AppSettings["PswSender"].ToString();
                string _Smtp = ConfigurationManager.AppSettings["SMTP"].ToString();
                int _Port = int.Parse(ConfigurationManager.AppSettings["PORT"].ToString());

                var GenarateUserVerificationLink = "/Registro/UserVerification/" + activationCode;
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenarateUserVerificationLink);

                var fromMail = new MailAddress(_FromEmailAddress, "Cinema86 Verificación de correo de usuario"); // set your email  
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
                Message.Subject = "Registro Completado";
                Message.Body = "<br/> Su proceso de registro se ha completado." +
                               "<br/> haga clic por favor en la liga de abajo para verificar su cuenta" +
                               "<br/><br/><a href=" + link + ">" + link + "</a>";
                Message.IsBodyHtml = true;
                smtp.Send(Message);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: Registro
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {

            InicializaControles();

            return View();
        }

        #region Registration post method for data save  
        [HttpPost]
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Index(Usuario _usr)
        {
            try
            {
                CN86_Usuario objUsr = new CN86_Usuario();

                if (!ModelState.IsValid)//Valida si los campos cuentan con el estandar correcto.
                {
                    InicializaControles();
                    return View();
                }
                    //Primero valido que exista un usuario con el mismo correo.
                    string _emailSearch = _usr._eMail.ToLower().Trim();
                var IsExist = IsEmailExists(_emailSearch);

                if(IsExist)
                {
                    string _MsgRegistration = "Ya existe un usuario registrado con el correo " + _usr._eMail.ToLower().Trim() + " Intente con otro.";
                    ModelState.AddModelError("EmailExist", _MsgRegistration);
                    InicializaControles();
                    return View();
                }

                // email not verified on registration time  
                objUsr.eMail_Confirmed = false;

                objUsr.Id_Estatus = 0;
                objUsr.Id_Estado = _usr._Id_Estado;

                objUsr.Nombres = _usr._Nombres.ToUpper().Trim();
                objUsr.ApellidoPaterno = _usr._ApellidoPaterno.ToUpper().Trim();
                objUsr.ApellidoMaterno = _usr._ApellidoMaterno.ToUpper().Trim();

                objUsr.eMail_Confirmed = false;
                objUsr.eMail = _usr._eMail.ToLower().Trim();

                //it generate unique code     
                objUsr.ActivetionCode = Guid.NewGuid().ToString();
                //password convert  
                objUsr.Password = Cinema86Movie.Models.EncriptaPassword.EncriptaTexto(_usr._Password);
                objUsr.Fecha_Registro = DateTime.Now;
                objUsr.Fecha_Expiracion = DateTime.Now;

                objCon.CN86_Usuario.Add(objUsr);
                objCon.SaveChanges();

                string _fechaRegistro = objUsr.Fecha_Registro.ToString();//ToString("yyyy/MM/dd hh:mm:ss tt", DateTimeFormatInfo.InvariantInfo);
                string _fechaVigencia = _fechaRegistro;

                #region Send eMail Verification Link
                    SendEmailToUser(objUsr.eMail, objUsr.ActivetionCode);
                    SendEmailToSistemOperator(objUsr.eMail, objUsr.Nombres + "-" + 
                        objUsr.ApellidoPaterno + "-" + objUsr.ApellidoMaterno,
                        _fechaRegistro, _fechaVigencia);
                    ViewBag.Message = "Registro completado, verifique su cuenta de email. Presione Salir.";
                #endregion

                return View("Registration");
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View("Registration");
            }
        }
        #endregion
    }
}