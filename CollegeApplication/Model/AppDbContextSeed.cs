using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AppDbContextSeed
    {
		public static Task SeedAsync(AppDbContext context)
		{
			//se invierte la logica una vez que insertes datos en la base de datos
			if (!context.Students.Any())
			{
				var seedStudents = new List<Student>
				{
					new Student { CodeNumber = "M029761", FirstName = "Mario", LastName = "Lopez" },
					new Student { CodeNumber = "M029762", FirstName = "Rene", LastName = "Quinones" },
					new Student { CodeNumber = "M029763", FirstName = "Alejandra", LastName = "Flores" }
				};
		

				foreach (var student in seedStudents)
					{
						context.Students.Add(student);
					}
					context.SaveChanges();
				}

			if (!context.Courses.Any())
			{
				var seedCourses = new List<Course>
				{
					new Course { Title = "Chemistry", Credits = 6 },
					new Course { Title = "Spanish", Credits = 4 }
				};
		

				foreach (var course in seedCourses)
				{
					context.Courses.Add(course);
				}
				context.SaveChanges();
			}

			return Task.CompletedTask;
		}
	}
}
