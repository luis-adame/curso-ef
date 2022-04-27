using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Student
    {
		public int Id { get; set; }
		public string CodeNumber { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime EnrollmentDate { get; set; }
		public virtual ICollection<Enrollment> Enrollments { get; set; }

		public Student() { }

		public Student(string codeNumber, string firstName, string lastName)
		{
			CodeNumber = codeNumber;
			FirstName = firstName;
			LastName = lastName;
		}
	}
}
