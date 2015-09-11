using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mhotivo.ParentSite.Models
{
    public class MessageToTeacherModel
    {
        public string To { get; set; }
        public string Subject   { get; set; }
        public string Message { get; set; }
    }
}