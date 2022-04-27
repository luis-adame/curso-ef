using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class CourseRegistryDto
    {
		public string Title { get; set; }
		public int Credits { get; set; }
		public int Capacity { get; set; }

		public void Validation()
		{
			if (string.IsNullOrEmpty(Title))
				throw new ArgumentNullException("'Title' must not be empty.");

			if (Credits <= 0)
				throw new InvalidOperationException("'Credits' must be higher than 0.");

			if (Capacity <= 0)
				throw new InvalidOperationException("'Capacity' must be higher than 0.");
		}
	}
}
