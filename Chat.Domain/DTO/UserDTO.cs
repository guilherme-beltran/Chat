using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.DTO
{
    internal sealed class UserDTO
    {
        [Required(ErrorMessage = "Por favor, informe o nome do usuário.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Nome deve ter no minimo 3 caracteres e no maximo 30.")]
        public string Name { get; set; }
    }
}
