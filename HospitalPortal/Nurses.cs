using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HospitalPortal
{
	internal class Nurses:Employee,IPageable
	{
        public string Lavel { get; set; }

		public void Page()
		{
			Console.WriteLine($"Paging Nurse {LastName}");
		}

		public override void view()
		{
			Console.WriteLine();
			Console.WriteLine($"Detail for {EmployeeID}");
			Console.WriteLine($"{LastName} , {FirstName}");
			Console.WriteLine($"Nurse {Lavel}");
			Console.WriteLine();
		}
	}
}
