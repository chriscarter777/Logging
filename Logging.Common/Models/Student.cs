using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Common.Models;

     public class Student
     {
     public Student() { }

     public Student(string firstName, string lastName)
     {
          FirstName = firstName;
          LastName = lastName;
     }


     public string FirstName { get; set; }
     public string LastName { get; set; }
}
