using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_Avanzada.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress]
        [Display(Name = "Correo electronico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contrasena es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contrasena")]
        public string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(80)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(120)]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [StringLength(20)]
        [Display(Name = "Cedula")]
        public string Cedula { get; set; }

        [StringLength(20)]
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo electronico")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La {0} debe tener al menos {2} caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contrasena")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contrasena y su confirmacion no coinciden.")]
        [Display(Name = "Confirmar contrasena")]
        public string ConfirmPassword { get; set; }
    }
}