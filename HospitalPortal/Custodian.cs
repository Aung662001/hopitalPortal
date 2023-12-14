using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HospitalPortal
{
	internal class Custodian : Employee
	{
		public override void view()
		{
			Console.WriteLine();
			Console.WriteLine($"Detail for {EmployeeID}");
			Console.WriteLine($"{LastName} , {FirstName}");
			Console.WriteLine($"Custodian");
			Console.WriteLine();
		}
	}
}
