using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class NotificationCommentDisplayModel
    {
        public long CommentId { get; set; }

        [Display(Name = "Usuario")]
        public string Commenter { get; set; }

        [Display(Name = "Comentario")]
        public string CommentText { get; set; }

        [Display(Name = "Fecha")]
        public string CreationDate { get; set; }
    }

    public class NotificationCommentRegisterModel
    {

        [Display(Name = "Usuario")]
        public long Commenter { get; set; }

        [Display(Name = "Comentario")]
        public string CommentText { get; set; }

        public long Notification { get; set; }
    }

    public class NotificationCommentEditModel
    {
        public long CommentId { get; set; }

        [Display(Name = "Comentario")]
        public string CommentText { get; set; }
    }
}