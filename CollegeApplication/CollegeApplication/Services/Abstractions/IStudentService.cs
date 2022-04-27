using Model.Entities;
using Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeApplication.Services.Abstractions
{
    public interface IStudentService
    {
        StudentDto GetByCodeNumber(string codeNumber);
        public void RegisterStudent(StudentRegistryDto studentRegistry);
        public void AssignCourse(CourseAssignmentDto courseAssignment);
        public StudentEvaluationDto GetEvaluationByCodeNumber(string codeNumber);
        public void Evaluate(StudentEvaluationDto evaluation);
        public void Unsuscribe(EnrollmentEditDto enrollment);
    }
}
