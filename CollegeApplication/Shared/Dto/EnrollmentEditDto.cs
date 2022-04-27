using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class EnrollmentEditDto
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public string StudentCodeNumber { get; set; }
        public Boolean Active { get; set; }

        public void ValidateCourseId()
        {
            if (CourseId <= 0)
                throw new Exception("'Id' higher than 0.");
        }

        public void ValidateStudentCodeNumber()
        {
            if (string.IsNullOrEmpty(StudentCodeNumber))
                throw new ArgumentNullException("'StudentCodeNumber' must not be empty.");
        }
    }
}
