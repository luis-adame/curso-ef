using Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeApplication.Services.Abstractions
{
    public interface ICourseService
    {
        public void RegisterCourse(CourseRegistryDto courseRegistry);
        public List<CourseDto> GetAll();
        public List<CourseDto> GetAvailable(int studentId);
        public void EditCourse(CourseEditDto courseEdit);
        public void DeleteCourse(CourseEditDto courseDelete);
        public List<CourseDto> GetAvailable2(int studentId);
        public List<CourseDto> GetStudentCourses(int studentId);
    }
}
