using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Course
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public int Credits { get; set; }
		public virtual ICollection<Enrollment> Enrollments { get; set; }
		public int Capacity { get; set; }

		public Course() { }

		public Course(string title, int credits, int capacity)
		{
			Title = title;
			Credits = credits;
			Capacity = capacity;
		}
	}
}
