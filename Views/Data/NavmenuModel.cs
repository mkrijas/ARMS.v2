using System.Collections.Generic;
using MudBlazor;

namespace Views.Data
{
    public class NavMenuModel
    {
        public int ID { get; set; }
        public bool IsGroup { get; set; }
        public string Title { get; set; }
        public string ParentTitle { get; set; }
        public string href { get; set; }
        public string Icon { get; set; }
        public Color IconColor { get; set; }
        public bool Expanded { get; set; } = false;
        public string Description { get; set; }
        public List<NavMenuModel> navItems { get; set; } = new();
        public bool Visible { get; set; } = true;
    }



}
