using System.Collections.Generic;
using System.Web.Mvc;

namespace Mhotivo.Models
{
    public class DynamicListModel
    {
        public DynamicListModel()
        {
            Items = new List<SelectListItem>();
        }
        public List<SelectListItem> Items { get; set; }
    }
}