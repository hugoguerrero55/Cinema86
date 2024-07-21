using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Cinema86Movie.Models
{
    public class Login
    {        
        [Display(Name = "Correo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Correo electrónico es requerido")]
        public string _eMail { get; set; }
     
        [Display(Name = "Contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contraseña es requerida")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Se necesitan al menos 6 caracteres")]
        public string _Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool Rememberme { get; set; }
    }
}