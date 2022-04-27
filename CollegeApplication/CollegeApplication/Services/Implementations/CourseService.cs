using CollegeApplication.Services.Abstractions;
using Model;
using Model.Entities;
using Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeApplication.Services.Implementations
{
    public class CourseService : ICourseService
    {
		private readonly AppDbContext context = new AppDbContext();

		public void RegisterCourse(CourseRegistryDto courseRegistry)
		{
			var course = new Course(courseRegistry.Title, courseRegistry.Credits, courseRegistry.Capacity);

			try
			{
				context.Courses.Add(course);
				context.SaveChanges();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public List<CourseDto> GetAll()
		{
			var courses = context.Courses.Select(c => new CourseDto
			{
				Id = c.Id,
				Title = c.Title,
				Credits = c.Credits
			}).ToList();

			if (!courses.Any())
				throw new Exception("There are no courses available.");

			return courses;
		}

		//Ya no lo uso
		public List<CourseDto> GetAvailable(int studentId)
		{
			var courses = context.Courses
				.Where(c => !c.Enrollments.Select(c => c.StudentId).Contains(studentId))
				.Select(c => new CourseDto
			{
				Id = c.Id,
				Title = c.Title,
				Credits = c.Credits
			}).ToList();

			if (!courses.Any())
				throw new Exception("There are no courses available.");

			return courses;
		}

		public void EditCourse(CourseEditDto courseEdit)
        {
			var course = context.Courses.Where(c => c.Id.Equals(courseEdit.CourseId)).FirstOrDefault();

			if (course is null)
				throw new Exception();
			else
            {
				course.Title = courseEdit.Title;
				course.Credits = courseEdit.Credits;
				course.Capacity = courseEdit.Capacity;

				context.SaveChanges();
			}
        }

		public void DeleteCourse(CourseEditDto courseDelete)
        {
			//var enrollments = context.Enrollments.Where(c => c.CourseId.Equals(courseDelete.CourseId)).ToList();
			var course = context.Courses.Where(c => c.Id.Equals(courseDelete.CourseId)).FirstOrDefault();

            if (course is null)
				throw new Exception("This Course does not exist.");
            else
            {
				context.Courses.Remove(course);

				context.SaveChanges();
            }
		}

		public List<CourseDto> GetAvailable2(int studentId)
		{
			var courses = context.Courses
				.Where(c => !c.Enrollments.Where(c => c.Active).Select(c => c.StudentId).Contains(studentId))
				.Select(c => new CourseDto
				{
					Id = c.Id,
					Title = c.Title,
					Credits = c.Credits,
					Capacity = c.Capacity - c.Enrollments.Where(a => a.Course.Id.Equals(c.Id) && a.Active).Count()
				}).ToList();

			if (!courses.Any())
				throw new Exception("There are no courses available.");

			return courses;
		}

		public List<CourseDto> GetStudentCourses(int studentId)
		{
			var courses = context.Courses
				.Where(c => c.Enrollments.Where(c => c.Active).Select(c => c.StudentId).Contains(studentId))
				.Select(c => new CourseDto
				{
					Id = c.Id,
					Title = c.Title,
					Credits = c.Credits
				}).ToList();

			if (!courses.Any())
				throw new Exception("There are no courses available.");

			return courses;
		}
	}
}
