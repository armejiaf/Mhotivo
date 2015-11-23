using System;
using System.Collections.Generic;
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

        public virtual string AcademicCourse { get; set; }
    }

    public class HomeworksModel
    {
        public HomeworksModel()
        {
            PastHomeworks = new List<HomeworkModel>();
            CurrentHomeworks = new List<HomeworkModel>();
            FutureHomeworks = new List<HomeworkModel>();
        }
        public List<HomeworkModel> PastHomeworks { get; set; }
        public List<HomeworkModel> CurrentHomeworks { get; set; }
        public List<HomeworkModel> FutureHomeworks { get; set; }
    }
}