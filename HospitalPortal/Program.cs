using HospitalPortal;

internal class Program
{
	public static string employeeFilePath = @"employeeFiles";
	public static List<Employee> employees = new List<Employee>();
	public static bool running = true;
	private static void Main(string[] args)
	{
		Intialize();
		while (running)
		{
			Console.Write(">");
			string input = Console.ReadLine();
			processInput(input);
		}
		Console.WriteLine("Press any key to exit.");
		Console.ReadLine();
		ReadKey();
		
	}

	private static void processInput(string? input)
	{
		switch (input)
		{
			case "help":
				printHelp();
				break;
			case "add":
				addEmployee();
				break;
			case "load":
				loadEmployee();
				break;
			case "view":
				viewEmployee();
				break;
			case "remove":
				removeEmployee();
				break;
			case "allids":
				ViewAllIds();
				break;
			case "exit":
				closeApp();
				break;
			case "page":
				pageEmployee();
				break;
			default:
				invalidCommand();
				break;

		}
	}

	private static void pageEmployee()
	{
			if (employees.Count <= 0)
			{
				Console.WriteLine("There is no employee.");
				return;
			}
			foreach (Employee employee in employees)
			{
				if (employee is IPageable )
				{
					IPageable pageable = (IPageable)employee;
					pageable.Page();
				}
			}
	}

	private static void closeApp()
	{
		Console.WriteLine("Exiting...");
		ReadKey();
	}

	private static void ViewAllIds()
	{
		if(employees.Count <= 0)
		{
			Console.WriteLine("No Employee found.");
			return;
		}
		foreach(Employee emp in employees)
		{
			Console.WriteLine($"EmployeeId: {emp.EmployeeID}");
		}
	}

	private static void removeEmployee()
	{
		Console.Write("Enter employeeID:");
		string id = Console.ReadLine();
		string path = Path.Combine(employeeFilePath, id);
		if (!File.Exists(path))
		{
			Console.WriteLine($"Employee not exist with id {id}.");
			return;
		}
		try
		{
			File.Delete(path);
			Console.WriteLine($"Deleted employee with id {id} successfully");
			loadEmployee();
		} catch(Exception ex)
		{
			Console.WriteLine("Can't delete employee with {id}.Please contact to admin");
			Console.WriteLine(ex.Message);
			running = false;
		}
	}

	private static void viewEmployee()
	{
		if(employees.Count <= 0 )
		{
			Console.WriteLine("There is no employee.");
			return;
		}
		Console.Write("Enter employeeID:");
		string id = Console.ReadLine();
		try
		{
			bool foundId = false;
			foreach(Employee employee in employees)
			{
				if(employee.EmployeeID == id)
				{
					foundId = true;
					employee.view();
				}
			}
			if (!foundId)
			{
				Console.WriteLine($"Employee not found with Id {id}");
			}
		}catch(Exception ex) { Console.WriteLine("Can't find employee."); };
		
	}

	private static void addEmployee()
	{
		Console.WriteLine("");

		 Console.Write("Employee Id:");
		 string id = Console.ReadLine();
		 string filePath = Path.Combine(employeeFilePath, id);

			if(Path.Exists(filePath))
			{
				Console.WriteLine($"Employee already exist with Id {id}");
			    return;
			}

		Console.Write("First Name:");
		string fname = Console.ReadLine();
		Console.Write("Last Name:");
		string lname = Console.ReadLine();

		bool hasValidJobTitle = false;
		string jobTitle = "";
		while (!hasValidJobTitle)
		{
			 Console.Write("Job Title (doctor,nurse,custodian):");
			 jobTitle = Console.ReadLine();

			if (jobTitle == "doctor" || jobTitle == "nurse" || jobTitle == "custodian")
				hasValidJobTitle = true;
			else
				Console.WriteLine("Invalid job title. Please try again !");
		}

		switch (jobTitle)
		{
			case "doctor":
				addDoctor(id,fname,lname);
				break;
			case "nurse":
				addNurse(id,fname,lname);
				break;
			case "custodian":
				addCustodian(id,fname,lname);
				break;
			default:
				Console.WriteLine("Invalid input. Please try aganin!.");
				addEmployee();
				break;
		}

	}

	private static void addCustodian(string id, string? fname, string? lname)
	{
		string[] custodianData =
		{
			"custodian" ,id,fname,lname
		};
		WriteEmployeeFile(id, custodianData);
	}

	private static void addNurse(string id, string? fname, string? lname)
	{
		Console.Write("Please enter nurse's lavel (RN,LPN,etc...):");
		string lavel = Console.ReadLine();
		string[] nurseData =
		{
			"nurse",id,fname,lname,lavel
		};
		WriteEmployeeFile(id,nurseData);
	}

	private static void addDoctor(string id,string fname,string lname)
	{
		Console.Write("Please enter doctor's Speciality (Surgeon,Cardiologist,Orthopedic,Ophthalmologist):");
		string speciality = Console.ReadLine();

		string[] doctorData =
		{
			"doctor",id,fname,lname,speciality
		};
		WriteEmployeeFile(id, doctorData);
	}

	private static void WriteEmployeeFile(string id, string[] doctorData)
	{
		try
		{
			string path = Path.Combine(employeeFilePath, id);
			File.WriteAllLines(path,doctorData);
			Console.WriteLine($"Added employee {id} successfully!");
			loadEmployee();
		}catch (Exception ex)
		{
			Console.WriteLine("Error creating employee. Please try again or contact to your admin.");
			Console.WriteLine($"Error : {ex.Message}");
			running = false;
		}
	}

	private static void loadEmployee()
	{
		employees.Clear();
		string[] filesList = Directory.GetFiles(employeeFilePath);
		foreach(string employees in filesList)
		{
			string[] employee = File.ReadAllLines(employees);
			switch (employee[0])
			{
				case "doctor":
					loadDoctor(employee);
					break;
				case "nurse":
					loadNurse(employee);
					break;
				case "custodian":
					loadCustodian(employee);
					break;
				default:
					Console.WriteLine("Error at loading employees.");
					break;
			}
		}
		Console.WriteLine($"Loaded {employees.Count} employees");
	}

	private static void loadCustodian(string[] employee)
	{
		Custodian custodian = new Custodian
		{
			EmployeeID = employee[1],
			FirstName = employee[2],
			LastName = employee[3],
		};
		employees.Add(custodian);
	}

	private static void loadNurse(string[] employee)
	{
		Nurses nurse = new Nurses
		{
			EmployeeID = employee[1],
			FirstName = employee[2],
			LastName = employee[3],
			Lavel = employee[4]
		};
		employees.Add(nurse);
	}

	private static void loadDoctor(string[] employee)
	{
		Doctor doctor = new Doctor
		{
			EmployeeID = employee[1],
			FirstName = employee[2],
			LastName = employee[3],
			Speciality = employee[4]
		};
		employees.Add(doctor);
			
	}

	private static void invalidCommand()
	{
		Console.WriteLine();
		Console.WriteLine("Command not regonized ,please try again .");
		Console.WriteLine("Type \"help\" for avaiable commands");
		Console.WriteLine();
	}

	private static void printHelp()
	{
		Console.WriteLine("\thelp - avaiable commands.");
		Console.WriteLine("\tadd - add new employee");
		Console.WriteLine("\tremove - remove an employee");
		Console.WriteLine("\tload - load existing employees from file");
		Console.WriteLine("\tview - view an employee with employeeID");
		Console.WriteLine("\tpage - page All medical employees");
		Console.WriteLine("\tallids - view All  employees Ids");
		Console.WriteLine("\texit - close the application");
	}

	private static void Intialize()
	{
		try
		{
			Directory.CreateDirectory(employeeFilePath);
		    printHeader();
			loadEmployee();
		}
		catch(Exception ex)
		{
			Console.WriteLine("Error Creating Directory.");
			Console.WriteLine($"Message: ${ex.Message}");
			running = false;
		}
	}

	private static void printHeader()
	{
		Console.WriteLine("-----------------------------");
		Console.WriteLine("Wellcome to the hospital mannagement portal.");
		Console.WriteLine("Type \"help\" for avaiable commands");
		Console.WriteLine("-----------------------------");
	}
	private static void ReadKey()
	{
		System.Environment.Exit(0);
	}
};