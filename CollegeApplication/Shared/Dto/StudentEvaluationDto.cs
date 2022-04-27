using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class StudentEvaluationDto
    {
        public StudentDto Student { get; set; }
        public List<StudentEnrollmentDto> Enrollments { get; set; }
    }
}
