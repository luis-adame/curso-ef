using CollegeApplication.Services.Abstractions;
using CollegeApplication.Services.Implementations;
using Shared;
using Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeApplication
{
    public class Menu
    {
		private readonly IStudentService _studentService;
		private readonly ICourseService _courseService;

		public Menu()
		{
			_studentService = new StudentService();
			_courseService = new CourseService();
		}

		public bool Show()
		{
			Console.Clear();
			Console.WriteLine("1. Student registration");
			Console.WriteLine("2. Course registration");
			Console.WriteLine("3. Course assignment");
			Console.WriteLine("4. Evaluate student performance");
			Console.WriteLine("5. Consult student performance");
			Console.WriteLine("6. Edit Course");
			Console.WriteLine("7. Unsuscribe student.");
			Console.WriteLine("8. Delete course");
			Console.WriteLine("X. Any other key to exit");
			Console.Write("Your Option: ");

			switch (Console.ReadLine())
			{
				case "1":
					Console.Clear();
					RegisterStudent();
					break;
				case "2":
					Console.Clear();
					RegisterCourse();
					break;
				case "3":
					Console.Clear();
					AssignCourse();
					break;
				case "4":
					Console.Clear();
					EvaluateStudentPerformance();
					break;
				case "5":
					Console.Clear();
					ConsultStudentPerformance(); Console.ReadLine();
					break;
				case "6":
					Console.Clear();
					EditCourse();
					break;
				case "7":
					Console.Clear();
					UnsuscribeStudent();
					break;
				case "8":
					Console.Clear();
					DeleteCourse();
					break;

				default:
					return false;
			}

			return true;
		}

		private void RegisterStudent()
		{
			var studentRegistry = new StudentRegistryDto();

			Console.WriteLine("Please enter the required information to register a new student");
			Console.Write("Student Code Number: ");
			studentRegistry.CodeNumber = Console.ReadLine();
			Console.Write("First Name: ");
			studentRegistry.FirstName = Console.ReadLine();
			Console.Write("Last Name: ");
			studentRegistry.LastName = Console.ReadLine();

			try
			{
				studentRegistry.Validation();
				_studentService.RegisterStudent(studentRegistry);
				Console.WriteLine("Student registered correctly.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				Console.WriteLine("Press any key to continue.");
				Console.ReadLine();
			}
		}

		private void RegisterCourse()
		{
			var courseRegistry = new CourseRegistryDto();

			Console.WriteLine("Please enter the required information to register a new course");
			Console.Write("Title: ");
			courseRegistry.Title = Console.ReadLine();
			Console.Write("Credits: ");
			var input = Console.ReadLine();

			var isValid = Int32.TryParse(input, out int credits);
			if (!isValid)
			{
				Console.WriteLine("Please enter a valid number.\nPress any key to continue.");
				Console.ReadLine();
				return;
			}

			Console.Write("Capacity: ");
			input = Console.ReadLine();

			isValid = Int32.TryParse(input, out int capacity);
			if (!isValid)
			{
				Console.WriteLine("Please enter a valid number.\nPress any key to continue.");
				Console.ReadLine();
				return;
			}

			try
			{
				courseRegistry.Credits = credits;
				courseRegistry.Capacity = capacity;
				courseRegistry.Validation();

				_courseService.RegisterCourse(courseRegistry);
				Console.WriteLine("Course registered correctly.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				Console.WriteLine("Press any key to continue.");
				Console.ReadLine();
			}
		}

		private void AssignCourse()
		{
			var courseAssignment = new CourseAssignmentDto();

			Console.WriteLine("Please enter the required information to assign a course");
			Console.Write("Student Code Number: ");

			try
			{
				courseAssignment.StudentCodeNumber = Console.ReadLine();
				courseAssignment.ValidateStudentCodeNumber();

				var student = _studentService.GetByCodeNumber(courseAssignment.StudentCodeNumber);
				//var enrollments = _studentService.GetStudentEnrollments(student.Id);
				//var courses = _courseService.GetAll();
				var courses = _courseService.GetAvailable2(student.Id);

				Console.WriteLine($"\nStudent Information\nStudent Code Number: {student.CodeNumber}\tName: {student.FirstName} {student.LastName}");
				Console.WriteLine("\nAvailable courses");

				//foreach (var enrollment in enrollments)
				//	courses.RemoveAll(c => c.Id.Equals(enrollment.Id));

				foreach (var course in courses)
				{
					if(course.Capacity > 0)
						Console.WriteLine($"Id: {course.Id}\tTitle: {course.Title}\tCredits: {course.Credits}\tAvailability: {course.Capacity}");
				}

				Console.WriteLine("\nPlease choose an option");
				Console.Write("Course Id:");
				var input = Console.ReadLine();

				if (Int32.TryParse(input, out int courseId))
					courseAssignment.CourseId = courseId;
				else
					throw new Exception("Course Id must be a number.");

				courseAssignment.ValidateCourseId();
				_studentService.AssignCourse(courseAssignment);

				Console.WriteLine("Student has been assign to the course.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				Console.WriteLine("Press any key to continue.");
				Console.ReadLine();
			}
		}

		private StudentEvaluationDto ConsultStudentPerformance()
		{
			Console.WriteLine("");
			Console.Write("Student Code Number: ");
			var input = Console.ReadLine();

			try
			{
				var evaluation = _studentService.GetEvaluationByCodeNumber(input);

				Console.WriteLine($"\nStudent Information\nStudent code number: {evaluation.Student.CodeNumber}");
				Console.WriteLine();

				var counter = 0;
				foreach (var enrollment in evaluation.Enrollments)
				{
					var grade = enrollment.Grade == null ? "-" : enrollment.Grade.ToString();
					Console.WriteLine($"{counter++}. Course: {enrollment.Title}\t Grade: \t{grade}");
				}

				return evaluation;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		private void EvaluateStudentPerformance()
		{
			var next = false;
			var isValidUpdate = false;
			var evaluation = ConsultStudentPerformance();

			if (evaluation is null)
				return;

			Console.WriteLine("\n**Grade options are the following: A-B-C-D-F");

			do
			{
				Console.WriteLine("\nPlease choose a course to evaluate.");
				Console.Write("Course Id: ");
				var inputId = Console.ReadLine();
				var isValid = Int32.TryParse(inputId, out var id);

				if (!isValid)
				{
					Console.WriteLine("You must enter a valid number for course Id.");
					return;
				}
				//Esto es nuevo
				var selectedEnrollment = evaluation.Enrollments.ElementAt(id);

				//var exists = evaluation.Enrollments.Exists(e => e.CourseId.Equals(id));
				var exists = evaluation.Enrollments.Exists(e => e.CourseId.Equals(selectedEnrollment.CourseId));
		
				if(!exists)
				{
					Console.WriteLine("The course you entered is not correct.");
					return;
				}

				Console.Write("Assign a grade: ");
				var inputGrade = Console.ReadLine();
				isValid = Enum.TryParse(inputGrade, out Grade grade);

				if (!isValid)
				{
					Console.WriteLine("You must enter a valid Grade.");
					return;
				}

				//var index = evaluation.Enrollments.FindIndex(e => e.CourseId.Equals(id));
				var index = evaluation.Enrollments.FindIndex(e => e.CourseId.Equals(selectedEnrollment.CourseId));
				evaluation.Enrollments.ElementAt(index).Grade = grade;

				Console.WriteLine("Do you want to assign another grade? (y/n)");
				var input = Console.ReadLine();

				next = input.ToLower().Equals("y");

				if (!next)
					isValidUpdate = true;

			} while (next);

			if (!isValidUpdate)
				return;

			_studentService.Evaluate(evaluation);
		}

		private void EditCourse()
        {
			var courseEdit = new CourseEditDto();
			try
			{
				var courses = _courseService.GetAll();

				foreach (var course in courses)
				{
					Console.WriteLine($"Id: {course.Id}\tTitle: {course.Title}\tCredits: {course.Credits}");
				}

				Console.WriteLine("\nPlease choose an option");
				Console.Write("Course Id:");
				var input = Console.ReadLine();

				if (Int32.TryParse(input, out int courseId))
					courseEdit.CourseId = courseId;
				else
					throw new Exception("Course Id must be a number.");

				courseEdit.ValidateCourseId();

				Console.Write("Title: ");
				courseEdit.Title = Console.ReadLine();

				Console.Write("Credits: ");
				var creditsInput = Console.ReadLine();

				if (Int32.TryParse(creditsInput, out int credits))
					courseEdit.Credits = credits;
				else
					throw new Exception("Credits must be a number.");

				Console.Write("Capacity: ");
				var capacityInput = Console.ReadLine();

				if (Int32.TryParse(capacityInput, out int capacity))
					courseEdit.Capacity = capacity;
				else
					throw new Exception("Capacity must be a number.");

				courseEdit.Validation();

				_courseService.EditCourse(courseEdit);
				Console.WriteLine("Course updated.");

			}
			catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
            }
			finally
			{
				Console.WriteLine("Press any key to continue.");
				Console.ReadLine();
			}
		}

		private void UnsuscribeStudent()
        {
			var bajaEstudiante = new EnrollmentEditDto();

			Console.WriteLine("Student Code Number: ");
			var codeInput = Console.ReadLine();
			bajaEstudiante.StudentCodeNumber = codeInput;

			try
			{
				bajaEstudiante.ValidateStudentCodeNumber();
				var estudiante = _studentService.GetByCodeNumber(bajaEstudiante.StudentCodeNumber);

				if (estudiante is null)
					throw new Exception("Student does not exist.");
				else
					bajaEstudiante.StudentId = estudiante.Id;

				var cursos = _courseService.GetStudentCourses(estudiante.Id);

                foreach (var curso in cursos)
                {
					Console.WriteLine($"{curso.Id}\t{curso.Title}");
                }

				Console.WriteLine("Select course Id: ");
				var courseInput = Console.ReadLine();

				if (Int32.TryParse(courseInput, out int courseId))
					bajaEstudiante.CourseId = courseId;
				else
					throw new Exception("Course Id must be a number.");

				bajaEstudiante.ValidateCourseId();
				bajaEstudiante.Active = false;

				_studentService.Unsuscribe(bajaEstudiante);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
            finally
            {
				Console.WriteLine("Press any key to continue.");
				Console.ReadLine();
			}

        }

		public void DeleteCourse()
        {
			var courseDelete = new CourseEditDto();
			try
			{
				var courses = _courseService.GetAll();

				foreach (var course in courses)
				{
					Console.WriteLine($"Id: {course.Id}\tTitle: {course.Title}\tCredits: {course.Credits}");
				}

				Console.WriteLine("\nPlease choose an option");
				Console.Write("Course Id:");
				var input = Console.ReadLine();

				if (Int32.TryParse(input, out int courseId))
					courseDelete.CourseId = courseId;
				else
					throw new Exception("Course Id must be a number.");

				courseDelete.ValidateCourseId();

				_courseService.DeleteCourse(courseDelete);
				Console.WriteLine("Course deleted.");

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				Console.WriteLine("Press any key to continue.");
				Console.ReadLine();
			}
		}
	}
}
