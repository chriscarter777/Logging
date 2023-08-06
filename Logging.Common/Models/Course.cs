using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Common.Models;

public class Course
{
     public Course() { }

     public Course(int grade, string name)
     {
          Grade = grade;
          Name = name;
     }


     public int Grade { get; set; }
     public string Name { get; set; }
}
