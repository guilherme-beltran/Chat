using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.DTO
{
    internal class MessageDTO
    {
        [Required(ErrorMessage = "Origem da mensagem não informada.")]
        public string From { get; set; }
        public string To { get; set; }
        [Required(ErrorMessage = "Mensagem não informada.")]
        public string Content { get; set; }
    }
}
