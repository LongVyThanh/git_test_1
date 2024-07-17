using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baitap
{
    internal class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClassId { get; set; }

        public static List<Student> GetAll()
        {
            return new List<Student>()
            {
                new Student { Id = 11, Name = "st1", ClassId = 1},
                new Student { Id = 12, Name = "st2", ClassId = 1},
                new Student { Id = 13, Name = "st3", ClassId = 2},
                new Student { Id = 14, Name = "st4", ClassId = 4},
                new Student { Id = 15, Name = "st6", ClassId = 6}
            };
        }
    }
}
