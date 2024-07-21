using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cinema86Movie.Models
{
    public class OlvidoPassword
    {
        [Display(Name = "Correo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Correo electrónico es requerido")]
        public string _eMail { get; set; }
    }
}