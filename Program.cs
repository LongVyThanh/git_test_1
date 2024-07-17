using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Try101LinqSamples;

namespace baitap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //query tu nhieu nguon hay join nen dung query syntax
            //Bai 1
            //cross join
            //var CrossJoinResult1 = Student.GetAll()
            //    .SelectMany(cls => Class.GetAll(),
            //        (std, cls) => new
            //        {
            //            StudentId = std.Id,
            //            ClassId = cls.Id
            //        })
            //     .OrderBy(x => x.ClassId);

            //var CrossJoinResult2 = Student.GetAll()
            //    .Join(Class.GetAll(),
            //        std => 1,
            //        cls => 1,
            //        (std, cls) => new
            //        {
            //            StudentId = std.Id,
            //            ClassId = cls.Id
            //        })
            //    .OrderBy(x => x.ClassId);

            //var CrossJoinResult3 = from cls in Class.GetAll()
            //                       from std in Student.GetAll()
            //                       orderby cls.Id
            //                       select new
            //                       {
            //                           StudentId = std.Id,
            //                           ClassId = cls.Id
            //                       };

            //foreach (var item in CrossJoinResult2)
            //{
            //    Console.WriteLine($"{item.StudentId}, {item.ClassId}");
            //}
            //dem so hoc sinh o tat ca class ke ca ko co hs
            Console.WriteLine("---cau 1:");
            var cau1 = Class.GetAll().GroupJoin(
                Student.GetAll(),
                cls => cls.Id,
                std => std.ClassId,
                (cls, stdg) => new { cls.Name, count = stdg.Count() }
            );

            var CrossJoinResult2 = Class.GetAll()
                .Join(Student.GetAll(),
                    cls => cls.Id,
                    std => std.ClassId,
                    (cls, std) => cls)
                .Concat(Class.GetAll())
                .GroupBy(x => x.Name);

            //foreach (var item in CrossJoinResult2)
            //    Console.WriteLine($"{item.Key}, {item.Count() - 1}");

            var cau1_3 = Class.GetAll().SelectMany( 
                x => Student.GetAll(), 
                (cls, std) => {
                    if (cls.Id == std.ClassId)
                        return new {className = cls.Name, studentName = std.Name};
                    return new { className = cls.Name, studentName = ""};
                }
            ).GroupBy(
                x => x.className,
                (key, g) => {
                    var res = g.Where(x => x.studentName != "");
                    return new { Name = key, count = res.Count() };
                }
            );

            foreach (var item in cau1_3)
                Console.WriteLine($"{item.Name}, {item.count}");

            //in ra info hoc sinh va info class, neu hs ko thuoc class nao thi in ko co lop
            Console.WriteLine("---cau 2:");
            var cau2 = from s in Class.GetAll()
                       join c in Student.GetAll() on s.Id equals c.ClassId into temp
                       let clsCount = temp.Count()
                       from temp2 in temp.DefaultIfEmpty()
                       select new
                       {
                           clsCount,
                           stuName = s.Name,
                           clsName = temp2 == null ? "ko co ten lop" : temp2.Name
                       };
            foreach (var item in cau2)
            {
                Console.WriteLine($"{item.stuName}, {item.clsName}, {item.clsCount}");
            }
            //in them count cua class

            ////Bai 2
            ////gruop join
            //var groupJoin = Class.GetAll()
            //.GroupJoin(Student.GetAll(),
            //    cls => cls.Id,
            //    std => std.ClassId,
            //    (cls, stdGroup) => new
            //    {
            //        Students = stdGroup,
            //        ClassName = cls.Name
            //    }
            //);

            //foreach (var item in groupJoin)
            //{
            //    Console.WriteLine(item.ClassName + " " + item.Students.Count());
            //}

            ////Bai 3
            ////inner join
            //var Join = Student.GetAll()
            //    .Join(Class.GetAll(),
            //        std => std.ClassId,
            //        cls => cls.Id,
            //        (std, cls) => new
            //        {
            //            ClassId = cls.Id,
            //            ClasName = cls.Name,
            //            Students = std.Name
            //        }
            //     );
            //var gb = Join.GroupBy(x => x.ClassId);

            //foreach (var item in gb)
            //{
            //    Console.WriteLine(item.Key + " " + item.Count());
            //}

            ////Bai 4
            //var gb2 = Join.GroupBy(x => x.ClasName);

            //foreach (var item in gb2)
            //{
            //    Console.WriteLine(item.Key + " " + item.Count());
            //}

            ////Bai 5
            //foreach (var item in groupJoin)
            //{
            //    Console.Write(item.ClassName + " ");
            //    string str = item.Students.FirstOrDefault() == null ? "null" : item.Students.FirstOrDefault().Name;
            //    Console.WriteLine(str);
            //}

            ////Phan 2
            //var list1 = new[] {
            //    new { id = 1, name = "a"},
            //    new { id = 2, name = "b"},
            //    new { id = 3, name = "c"}
            //};
            //var list2 = new[] {
            //    new { id = 2, name = "b"},
            //    new { id = 4, name = "d"},
            //    new { id = 5, name = "e"},
            //};

            //var list3 = list1.Where(x => x.id == 2).ToList();
            //list3.ForEach(x => Console.WriteLine(x));

            //var list4 = list1.Where(x => x.id == 1 || x.id == 3).ToList();
            //list4.ForEach(x => Console.Write(x));

            ////left join
            //Console.WriteLine("\n---left join");
            //var qry = Class.GetAll()
            //          .GroupJoin(
            //              Student.GetAll(),
            //              cls => cls.Id,
            //              std => std.ClassId,
            //              (x, y) => new { Cls = x, Stds = y }
            //          )
            //          .SelectMany(
            //               x => x.Stds.DefaultIfEmpty(),
            //               (x, y) => new
            //               {
            //                   ClassName = x?.Cls.Name,
            //                   StudentId = y?.Id
            //               }
            //          );
            //foreach (var i in qry)
            //{
            //    Console.WriteLine(i.ClassName + " " + i.StudentId);
            //}

            ////right join
            //Console.WriteLine("---right join:");
            //var qry2 = Student.GetAll().GroupJoin(
            //          Class.GetAll(),
            //          std => std.ClassId,
            //          cls => cls.Id,
            //          (x, y) => new { Cls = y, Std = x })
            //          .SelectMany(
            //               x => x.Cls.DefaultIfEmpty(),
            //               (x, y) => new { ClassName = y?.Name, StudentId = x?.Std.Id }
            //          );
            //foreach (var i in qry2)
            //    Console.WriteLine(i.ClassName + " " + i.StudentId);

            ////full join
            //Console.WriteLine("---full join:");
            //var fullJoin = qry.Union(qry2);
            //foreach (var i in fullJoin)
            //    Console.WriteLine(i.ClassName + " " + i.StudentId);

            ////Exercise
            //string str1 = string.Join(", ",
            //    "Davis, Clyne, Fonte, Hooiveld, Shaw, Davis"
            //    .Split(',')
            //    .Select((item, index) => index + 1 + "." + item)
            //    .ToArray());
            //Console.WriteLine("---Exercise: " + str1);

            ////datetime
            //Console.WriteLine("---Datetime");
            //var str2 = "Jason Puncheon, 06/26/1986; Jos Hooiveld, 04/22/1983; Kelvin Davis, 09/29/1976"
            //    .Split(';')
            //    .Select(s => s.Split(','))
            //    .Select(s => new { Name = s[0].Trim(), Dob = DateTime.Parse(s[1].Trim()) })
            //    .Select(s => new { Name = s.Name, Dob = s.Dob, Age = DateTime.Now.Year - s.Dob.Year })
            //    .OrderBy(s => s.Age);
            //foreach (var s in str2)
            //    Console.WriteLine(s.Name + " " + s.Age);
            //Console.WriteLine("---");

            //Hashtable hash = new Hashtable();
            //hash.Add(1, "sd");
            //hash.Add("ad", 2);

            ////orders by customer, year, and month. 
            //List<Customer> customers = Customers.CustomerList;
            //var customerOrderGroups = from c in customers
            //                          select
            //                          (
            //                          c.CompanyName,
            //                          YearGroups: from o in c.Orders
            //                                      group o by o.OrderDate.Year into yg
            //                                      select
            //                                      (
            //                                      Year: yg.Key,
            //                                      MonthGroups: from o in yg
            //                                                   group o by o.OrderDate.Month into mg
            //                                                   select (Month: mg.Key, Orders: mg)
            //                                      )
            //                          );
            //var customerOrder2 = customers.Select(a =>
            //{
            //    Console.WriteLine($"Customer Name: {a.CompanyName}");
            //    return a.Orders.GroupBy(b => b.OrderDate.Year)
            //        .Select(c =>
            //        {
            //            Console.WriteLine($"\tYear: {c.Key}");
            //            return c.GroupBy(d => d.OrderDate.Month)
            //                .Select(e =>
            //                    {
            //                        Console.WriteLine($"\t\tMonth: {e.Key}");
            //                        return e.Select(f =>
            //                            {
            //                                Console.WriteLine($"\t\t\tOrder: {f}");
            //                                return f;
            //                            });
            //                    });
            //        });
            //});
            ////foreach (var a in customerOrder2)
            ////{
            ////    foreach (var b in a)
            ////        foreach (var c in b)
            ////            foreach (var d in c)
            ////            { }
            ////}

            ////SelectMany
            //var customerOrder3 = customers.SelectMany(a =>
            //{
            //    Console.WriteLine($"Customer Name: {a.CompanyName}");
            //    return a.Orders.GroupBy(b => b.OrderDate.Year)
            //        .SelectMany(c =>
            //        {
            //            Console.WriteLine($"\tYear: {c.Key}");
            //            return c.GroupBy(d => d.OrderDate.Month)
            //                .SelectMany(e =>
            //                    {
            //                        Console.WriteLine($"\t\tMonth: {e.Key}");
            //                        return e;
            //                    });
            //        });
            //});
            ////foreach (var a in customerOrder3)
            ////{
            ////    Console.WriteLine($"\t\t\tOrder: {a}");
            ////}

            ////Group
            //string[] anagrams = { "from   ", " from", " earn ", "  last   ", " near ", " form  " };

            //var orderGroups = anagrams.GroupBy(
            //            w => w.Trim(),
            //            a => a.ToUpper()
            //            );
            //foreach (var set in orderGroups)
            //{
            //    Console.WriteLine(set.Key);
            //    foreach (var word in set)
            //    {
            //        Console.WriteLine($"\t{word}");
            //    }
            //}

            ////Aggregate
            //double startBalance = 100.0;

            //int[] attemptedWithdrawals = { 20, 10, 40, 50, 10, 70, 30 };

            //double endBalance =
            //    attemptedWithdrawals.Aggregate(startBalance,
            //        (balance, nextWithdrawal) =>
            //            ((nextWithdrawal <= balance) ? (balance - nextWithdrawal) : balance));

            //Console.WriteLine($"Ending balance: {endBalance}");

            ////zip
            //int[] vectorA = { 0, 2, 4 };
            //int[] vectorB = { 1, 3, 5, };

            //int dotProduct = vectorA.Zip(vectorB, (a, b) => a * b).Sum();

            //Console.WriteLine($"Dot product: {dotProduct}");

            ////Lazy
            //int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            //var lowNumbers = from n in numbers
            //                 where n <= 3
            //                 select n;

            //var temp = lowNumbers.ToList();

            //Console.WriteLine("First run numbers <= 3:");
            //foreach (int n in lowNumbers)
            //{
            //    Console.WriteLine(n);
            //}

            //for (int i = 0; i < 10; i++)
            //{
            //    numbers[i] = -numbers[i];
            //}

            //Console.WriteLine("Second run numbers <= 3:");
            //foreach (int n in lowNumbers)
            //{
            //    Console.WriteLine(n);
            //}

            ////exercise
            //var ret = new[]{
            //                    new { SNO=1,Name="Tom",Wages=200 },
            //                    new { SNO=1,Name="Tom",Wages=300 },
            //                    new { SNO=1,Name="Tom",Wages=400 },
            //                    new { SNO=2,Name="Rob",Wages=500 },
            //                    new { SNO=2,Name="Rob",Wages=600 },
            //                    new { SNO=3,Name="John",Wages=700 },
            //                    new { SNO=3,Name="John",Wages=800 },
            //                    new { SNO=3,Name="John",Wages=500 },
            //                    new { SNO=3,Name="John",Wages=600 }
            //                };
            //var mohan = ret.GroupBy(a => a.Name)
            //    .Select(b =>
            //    {
            //        return new { No = b.Key, Original = b.First(), Duplicate = b.Skip(1) };
            //    });
            //foreach (var mo in mohan)
            //    foreach (var n in mo.Duplicate)
            //        Console.WriteLine($"{n.SNO} + {mo.Original.Wages} + {n.Wages}");

            ////Exercise 8
            //Console.WriteLine("---Ex8: ");
            //string[] cities = { "ROME", "LONDON", "NAIROBI", "CALIFORNIA", "ZURICH", "NEW DELHI", "AMSTERDAM", "ABU DHABI", "PARIS" };
            //var city = cities.Select(a => a.Trim('"')).Where(b => b.First() == 'N' && b.Last() == 'I');

            //foreach (var c in city)
            //    Console.WriteLine(c);
            ////ex13
            //Console.WriteLine("---Ex13: ");
            //string ex13 = cities.Aggregate((a, b) => a + ", " + b);
            //string ex13_2 = String.Join(", ", cities);
            //Console.WriteLine(ex13);

            ////ex28
            //var ex28 = cities.OrderBy(a => a.Length).ThenBy(b => b);
            //foreach (var c in ex28)
            //    Console.WriteLine(c);

            ////string builder
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("hello");
            //sb.AppendFormat("{0}----{1}", "abc", "def");
            //Console.WriteLine("---string builder");
            //Console.WriteLine(sb);

            ////datetime
            //DateTime myDate1 = new DateTime(2015, 12, 25, 10, 30, 45);
            //DateTime myDate2 = new DateTime(2010, 12, 25, 10, 30, 45);
            //TimeSpan span1 = new TimeSpan(10, 0, 0, 0);
            //TimeSpan span2 = new TimeSpan(15, 0, 0, 0);
            //var myDate3 = myDate1 - myDate2;
            //Console.WriteLine(myDate3);

            ////foreach (TimeZoneInfo z in TimeZoneInfo.GetSystemTimeZones())
            ////{
            ////    Console.WriteLine(z.Id);
            ////}

            ////
            //var employees = new List<Employee>()
            //    {
            //        new Employee()
            //        {
            //            EmployeeId = 1,
            //            Name = "Alvin Johnston",
            //            Department = "Sales",
            //            Salary = 25000.00
            //        },
            //        new Employee()
            //        {
            //            EmployeeId = 2,
            //            Name = "Jessica Cuevas",
            //            Department = "Engineering",
            //            Salary = 65000.00
            //        },
            //    };

            //var employeeDepartmentGroups = employees.GroupBy(employee =>
            //{
            //    var salaryLevel = employee.Salary < 50000 ? "Entry-Level" :
            //                      employee.Salary >= 50000 && employee.Salary <= 85000 ? "Mid-Level" :
            //                      "Senior-Level";
            //    return salaryLevel;
            //});
            //Console.WriteLine("---group");
            //foreach (var e in employeeDepartmentGroups)
            //    Console.WriteLine(e.Key);


            //var number = new List<int>();
            //number.Add(1);
            //IEnumerable<int> query = number.Select(n => n * 10);
            //number.Add(2);
            //foreach (int n in query)
            //    Console.WriteLine(n + "|");

        }

    }
}

