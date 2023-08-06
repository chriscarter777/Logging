using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Common.Models;

public class Transcript
{
     public Transcript() { }

     public Transcript(List<Course> courses, Student student)
     {
          Courses = courses;
          Student = student;
     }


     public List<Course> Courses { get; set; }
     public Student Student { get; set; }
}
