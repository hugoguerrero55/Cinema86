//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cinema86Movie.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CN86_Usuario
    {
        public int Id_Usuario { get; set; }
        public int Id_Estatus { get; set; }
        public int Id_Estado { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string eMail { get; set; }
        public bool eMail_Confirmed { get; set; }
        public string Password { get; set; }
        public string ActivetionCode { get; set; }
        public string OTP { get; set; }
        public Nullable<System.DateTime> Fecha_Registro { get; set; }
        public Nullable<System.DateTime> Fecha_Expiracion { get; set; }
    
        public virtual CN86_Estado CN86_Estado { get; set; }
        public virtual CN86_Estatus_Usuario CN86_Estatus_Usuario { get; set; }
    }
}
