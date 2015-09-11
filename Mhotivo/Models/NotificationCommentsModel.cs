using System;
using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class NotificationCommentsModel
    {
        public long CommentId { get; set; }

        [Display(Name = "Usuario")]
        public string Username { get; set; }

        [Display(Name = "Comentario")]
        public string Comment { get; set; }

        [Display(Name = "Fecha")]
        public DateTime CreationDate { get; set; }

        public long NotificationId { get; set; }
    }
}