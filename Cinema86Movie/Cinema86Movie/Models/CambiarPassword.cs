using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cinema86Movie.Models
{
    public class CambiarPassword
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "OTP es requerido")]
        public string OTP { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Contraseña es requerida")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Se requiere al menos 6 caracteres")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirmacion de la contraseña es requerida")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La confirmación de la contrasela debe coincidir con la contraseña")]
        public string ConfirmPassword { get; set; }
    }
}