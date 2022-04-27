using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class StudentRegistryDto
    {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string CodeNumber { get; set; }

		public void Validation()
		{
			if (string.IsNullOrEmpty(FirstName))
				throw new ArgumentNullException("'Name' must not be empty.");
			if (string.IsNullOrEmpty(LastName))
				throw new ArgumentNullException("'LastName' must not be empty.");
			if (string.IsNullOrEmpty(CodeNumber))
				throw new ArgumentNullException("'CodeNumber' must not be empty.");
		}
	}
}
