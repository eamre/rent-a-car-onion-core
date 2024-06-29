using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Dynamic
{
    public class Sort
    {
        public string Field { get; set; }
        public string Dir { get; set; } //Ascending descending

        public Sort()
        {
            Field = string.Empty;
            Dir = string.Empty;
        }

        public Sort(string field, string dir)
        {
            Field = field;
            Dir = dir;
        }
    }
}
