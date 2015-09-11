using System;
using Mhotivo.Data.Entities;

namespace Mhotivo.ParentSite.Models
{
    public class HomeworkModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DeliverDate { get; set; }

        public float Points { get; set; }

        public virtual AcademicYearDetail AcademicYearDetail { get; set; }
    }
}