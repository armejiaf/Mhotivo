﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mhotivo.Data.Entities
{
    public class NotificationComments
    {
        [Key]
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual People Parent { get; set; }
    }
}
