using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace baitap
{
    internal class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static List<Class> GetAll()
        {
            return new List<Class>()
            {
                new Class { Id = 1, Name = "c1"},
                new Class { Id = 2, Name = "c2" },
                new Class { Id = 3, Name = "c3"},
                new Class { Id = 10, Name = "c10"}
            };
        }
    }
}
