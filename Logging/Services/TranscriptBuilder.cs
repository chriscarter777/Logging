using Logging.Common.Models;
using Logging.Common.Services;
using System.Diagnostics;

namespace Logging.Services;

public class TranscriptBuilder : ITranscriptBuilder
{
     private readonly ILogger<TranscriptBuilder> _logger;
     private string[] CourseNames = new string[]  { "Biology", "Chemistry", "Philosophy", "French", "History", "Art", "Physics", "Creative Writing", "Algebra", "Calculus", };
     private string[] FirstNames = new string[] { "John", "Jane", "James", "Mary", "Paul", "Cindy", "Steven", "Sylvia", "Thomas", "Theresa", };
     private string[] LastNames = new string[] { "Smith", "Kim", "Schmidt", "Dupont", "Hernandez", "White", "Johnson", "Santorini", "Brown", "Doe", };
     private readonly string _timestampFormat = "yyyy-MM-dd HH:mm:ss.sss";


     //ctor

     public TranscriptBuilder(ILogger<TranscriptBuilder> logger)
     {
          _logger = logger;
     }


     //public methods

     public List<Transcript> BuildMany(BuildTranscriptRequest buildRequest)
     {
          string method = nameof(BuildMany);
          List<Transcript> transcripts = new List<Transcript>();

          _logger.LogInformation("{timestamp} {method} invoked, session {session}.", DateTime.Now.ToString(_timestampFormat), method, buildRequest.Session);

          //show me details:
          //this does not work with the default logger
          _logger.LogDebug("{timestamp} {method} <-- {@requestObject}", DateTime.Now.ToString(_timestampFormat), method, buildRequest);
          //but this does:
          //Debug.WriteLine($"BuildTranscriptRequest: ThisMany={buildRequest.ThisMany} ThrowException={buildRequest.ThrowException}");

          try
          {
               if (buildRequest.ThrowException)
               {
                    throw new Exception("Something went wrong.");
               }

               for (int i = 0; i < buildRequest.ThisMany; i++)
               {
                    transcripts.Add(RandomTranscript());
               }
               return transcripts;
          }
          catch (Exception ex)
          {
               _logger.LogError(ex, "{timestamp} {method} session {session}", DateTime.Now.ToString(_timestampFormat), method, buildRequest.Session);
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
