using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Models
{
    public partial class ChinesePost
    {
        public int id { get; set; }
        public int LatestPostId { get; set; }
        //public DateTime PostDateTime { get; set; }
        public string Picture { get; set; }
    }
}
