using CollegeApplication.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
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
    public class StudentService : IStudentService
    {
        private readonly AppDbContext context = new AppDbContext();

        public void RegisterStudent(StudentRegistryDto studentRegistry)
        {
            var student = new Student(studentRegistry.CodeNumber, studentRegistry.FirstName, studentRegistry.LastName);

            try
            {
                context.Students.Add(student);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public StudentDto GetByCodeNumber(string codeNumber)
        {
            var student = context.Students.Select(s => new StudentDto
            {
                Id = s.Id,
                CodeNumber = s.CodeNumber,
                FirstName = s.FirstName,
                LastName = s.LastName,
            }).FirstOrDefault(s => s.CodeNumber.ToLower().Equals(codeNumber.ToLower()));

            if (student is null)
                throw new Exception("Student does not exist.");
            else
                return student;
        }

        public void AssignCourse(CourseAssignmentDto courseAssignment)
        {
            var student = context.Students.FirstOrDefault(s => s.CodeNumber.ToLower().Equals(courseAssignment.StudentCodeNumber.ToLower()));
            
            var course = context.Courses.Include(c => c.Enrollments).FirstOrDefault(c => c.Id.Equals(courseAssignment.CourseId));

            if (student is null)
                throw new ArgumentNullException("Student does not exist.");

            if (course is null)
                throw new ArgumentNullException("Course does not exist.");

            if (course.Enrollments.Where(e => e.Active).Select(e => e.StudentId).Contains(student.Id))
                throw new Exception("Student is already assigned to this course.");

            if (course.Enrollments.Where(e => e.Course.Id.Equals(course.Id) && e.Active).Count() >= course.Capacity)
                throw new Exception("Course not available.");

            var enrollment = context.Enrollments.Add(new Enrollment(student.Id, course.Id));

            student.Enrollments.Add(enrollment.Entity);
            course.Enrollments.Add(enrollment.Entity);
            context.SaveChanges();
        }

        public StudentEvaluationDto GetEvaluationByCodeNumber(string codeNumber)
        {
            var enrollments = context.Enrollments.Where(e => e.Student.CodeNumber.ToLower().Equals(codeNumber.ToLower()) && e.Active).Select(e => new StudentEnrollmentDto
            {
                CourseId = e.Course.Id,
                Title = e.Course.Title,
                Grade = e.Grade
            }).ToList();

            var evaluation = context.Students.Where(s => s.CodeNumber.ToLower().Equals(codeNumber.ToLower())).Select(s => new StudentEvaluationDto
            {
                Student = new StudentDto
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    CodeNumber = s.CodeNumber,
                },
                Enrollments = enrollments
            }).FirstOrDefault();

            if (evaluation is null)
                throw new ArgumentNullException("Student does not exist.");

            if (!enrollments.Any())
                throw new Exception("There are either no courses assigned to this student or all courses have been evaluated.");

            return evaluation;
        }

        public void Evaluate(StudentEvaluationDto evaluation)
        {
            var enrollments = context.Enrollments.Where(e => e.StudentId.Equals(evaluation.Student.Id) && e.Active).ToList();

            try
            {
                foreach (var enrollment in enrollments)
                {
                    var x = evaluation.Enrollments.First(e => e.CourseId.Equals(enrollment.CourseId));
                    enrollment.Grade = x.Grade;
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Unsuscribe(EnrollmentEditDto enrollmentEdit)
        {
            var enrollment = context.Enrollments.Where(e => e.StudentId.Equals(enrollmentEdit.StudentId) && e.CourseId.Equals(enrollmentEdit.CourseId)).FirstOrDefault();

            if (enrollment == null)
                throw new Exception("The Student is not assigned to this Course.");
            else
            {
                enrollment.Active = enrollmentEdit.Active;

                context.SaveChanges();
            }
        }
    }
}
