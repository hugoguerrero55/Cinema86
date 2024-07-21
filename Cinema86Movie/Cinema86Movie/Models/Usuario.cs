using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cinema86Movie.Models
{
    public partial class Usuario
    {
        public int _Id_Estatus { get; set; }

        [Display(Name = "Estado")]
        public int _Id_Estado { get; set; }

        [Display(Name = "Nombre(s)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre(s) es requerido")]
        public string _Nombres { get; set; }

        [Display(Name = "Paterno")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido paterno es requerido")]
        public string _ApellidoPaterno { get; set; }

        [Display(Name = "Materno")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido materno es requerido")]
        public string _ApellidoMaterno { get; set; }

        [Display(Name = "Correo")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",ErrorMessage = "Dirección de Correo electrónico incorrecta, por favor chéquela y corríjala.")]
        [StringLength(100, ErrorMessage = "Longitud máxima 100")]
        [DataType(DataType.EmailAddress)]
        public string _eMail { get; set; }

        [Display(Name = "Confirme correo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La confirmación del correo electrónico es requerido")]
        public bool _eMail_Confirmed { get; set; }

        [Display(Name = "Contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contraseña es requerida")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Se necesitan al menos 6 caracteres")]
        public string _Password { get; set; }

        [Display(Name = "Confirme contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirmación de contraseña es requerida")]
        [DataType(DataType.Password)]
        [Compare("_Password", ErrorMessage = "La confirmación de la contraseña debe coincidir con el campo 'Contraseña'")]
        public string _ConfirmPassword { get; set; }

        public string _ActivetionCode { get; set; }

        public DateTime _Fecha_Registro { get; set; }
    }
}