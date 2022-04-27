using System;
using System.Collections.Generic;

namespace EFCoreDatabaseFirst.Models
{
    public partial class Course
    {
        public Course()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Credits { get; set; }
        public int Capacity { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
