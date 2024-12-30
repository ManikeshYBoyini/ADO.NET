using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeAdoNet
{
    public class PostModel
    {
        public int Id { get; set; }
        public string KeyVal { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Section { get; set; }
        public int Year { get; set; }
        public string YearOfPost { get; set; }
        public int Month { get; set; }
    }
}
