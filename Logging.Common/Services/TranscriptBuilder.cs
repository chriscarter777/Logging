using Logging.Common.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Common.Services;

     public class TranscriptBuilder : ITranscriptBuilder
{
     private readonly ILogger<TranscriptBuilder> _logger;
     private string[] CourseNames = new string[] 
     {
          "Biology", "Chemistry", "Philosophy", "French", "History", "Art", "Physics", "Creative Writing", "Algebra", "Calculus",
     };
     private string[] FirstNames = new string[]
     {
           "John", "Jane", "James", "Mary", "Paul", "Cindy", "Steven", "Sylvia", "Thomas", "Theresa",
     };
     private string[] LastNames = new string[]
     {
           "Smith", "Kim", "Schmidt", "Dupont", "Hernandez", "White", "Johnson", "Santorini", "Brown", "Doe",
     };


     //ctor

     public TranscriptBuilder(ILogger<TranscriptBuilder> logger)
     {
          _logger = logger;
     }


     //public methods

     public Transcript Build(Guid session, bool throwException)
     {
          try
          {
               _logger.LogInformation($"{nameof(Build)} Session {session}");
               if (throwException)
               {
                    throw new Exception("You asked for it.");
               }

               return RandomTranscript();
          }
          catch (Exception ex)
          {
               _logger.LogError(ex, $"{nameof(Build)} Session {session}");
               throw;
          }
     }

     public List<Transcript> BuildMany(Guid session, int thisMany, bool throwException)
     {
          _logger.LogInformation($"{nameof(BuildMany)} session {session}");
          try
          {
               if (throwException)
               {
                    throw new Exception("You asked for it.");
               }

               List<Transcript> transcripts = new List<Transcript>();
               for (int i = 0; i < thisMany; i++)
               {
                    transcripts.Add(RandomTranscript());
               }
               return transcripts;
          }
          catch (Exception ex)
          {
               _logger.LogError(ex, $"{nameof(BuildMany)} session {session}");
               throw;
          }
     }


     //private methods

     private Course RandomCourse()
     {
          Random random = new Random();
          int index = random.Next(CourseNames.Length);
          int grade = random.Next(100);
          Course course = new Course(grade, CourseNames[index]);
          return course;
     }

     private Student RandomStudent()
     {
          Random random = new Random();
          int firstIndex = random.Next(FirstNames.Length);
          int lastIndex = random.Next(LastNames.Length);
          Student student = new Student(FirstNames[firstIndex], LastNames[lastIndex]);
          return student;
     }

     private Transcript RandomTranscript()
     {
          Random random = new Random();
          int courseCount = random.Next(2) + 4;
          List<Course> courses = new List<Course>();
          for (int i = 0; i < courseCount; i++)
          {
               courses.Add(RandomCourse());
          }
          Transcript transcript = new Transcript(courses, RandomStudent());
          return transcript;
     }
}
