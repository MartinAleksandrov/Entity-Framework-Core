using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            //Task - 3
            //var result = GetEmployeesFullInformation(context);
            //Console.WriteLine(result);

            //Task-4
            //var result4 = GetEmployeesWithSalaryOver50000(context);
            //Console.WriteLine(result4);

            //Task-5
            //var result5 = GetEmployeesFromResearchAndDevelopment(context);
            //Console.WriteLine(result5);

            //Task-6
            //var result6 = AddNewAddressToEmployee(context);
            //Console.WriteLine(result6);

            //Task-7
            //var result7 = GetEmployeesInPeriod(context);
            //Console.WriteLine(result7);

            //Task-8
            //var result8 = GetAddressesByTown(context);
            //Console.WriteLine(result8);

            //Task-9
            //var result9 = GetEmployee147(context);
            //Console.WriteLine(result9);

            //Task-10
            //var result10 = GetDepartmentsWithMoreThan5Employees(context);
            //Console.WriteLine(result10);

            //Task-11
            //var result11 = GetLatestProjects(context);
            //Console.WriteLine(result11);

            //Task-12
            //var result12 = IncreaseSalaries(context);
            //Console.WriteLine(result12);

            //Task-13
            //var result13 = GetEmployeesByFirstNameStartingWithSa(context);
            //Console.WriteLine(result13);

            //Task-14
            //var result14 = DeleteProjectById(context);
            //Console.WriteLine(result14);

            //Task-15
            var result15 = RemoveTown(context);
            Console.WriteLine(result15);
        }

        //Task-3
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MiddleName = e.MiddleName,
                    JobTitile = e.JobTitle,
                    Salary = e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitile} {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Task-4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .AsNoTracking()
                .OrderBy(e => e.FirstName)
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    Salary = e.Salary
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Task-5
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    DepartmentName = e.Department.Name,
                    Salary = e.Salary
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Task-6
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            //context.Addresses.Add(address);

            Employee? employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            employee!.Address = address;

            context.SaveChanges();

            string[] employees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address!.AddressText)
                .ToArray();

            return string.Join(Environment.NewLine, employees);

        }

        //Task-7
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Take(10)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    ManagerFirstName = e.Manager!.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects
                                .Where(e => e.Project.StartDate.Year >= 2001 &&
                                            e.Project.StartDate.Year <= 2003)
                                .Select(ep => new
                                {
                                    ProjectName = ep.Project.Name,
                                    StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                                    EndDate = ep.Project.EndDate.HasValue
                                                    ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                                    : "not finished"
                                })
                                .ToArray()
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Task-8
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town!.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    AddressText = a.AddressText,
                    TownName = a.Town!.Name,
                    EmployeeCount = a.Employees.Count
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        //Task-9
        public static string GetEmployee147(SoftUniContext context)
        {
            var employee147 = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Projects = e.EmployeesProjects
                                .OrderBy(ep => ep.Project.Name)
                                .Select(ep => ep.Project.Name)
                                .ToArray()
                });

            StringBuilder sb = new StringBuilder();


            foreach (var e in employee147)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                sb.AppendLine(string.Join(Environment.NewLine, e.Projects));
            }

            return sb.ToString().TrimEnd();
        }

        //Task-10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(e => e.Employees.Count > 5)
                .OrderBy(e => e.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employee = d.Employees.Select(e => new
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        JobTitle = e.JobTitle,
                    })
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToArray()

                });

            StringBuilder sb = new StringBuilder();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.DepartmentName} - {d.ManagerFirstName}  {d.ManagerLastName}");
                foreach (var e in d.Employee)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Task-11
        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    ProjectName = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var p in projects.OrderBy(p => p.ProjectName))
            {
                sb.AppendLine(p.ProjectName);
                sb.AppendLine(p.Description);
                sb.AppendLine(p.StartDate);
            }

            return sb.ToString().TrimEnd();
        }

        //Task-12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employee = context.Employees
                .Where(e => e.Department.Name == "Engineering" ||
                            e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" ||
                            e.Department.Name == "Information Services")
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Salary = e.Salary * (decimal)1.12
                })
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)
                .ToArray();

            //context.SaveChanges();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employee)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Task-13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employee = context.Employees
                .Where(e => e.FirstName.Substring(0, 2).ToLower() == "sa")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    FirtName = e.FirstName,
                    LastName = e.LastName,
                    Salary = e.Salary,
                    JobTitle = e.JobTitle
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employee)
            {
                sb.AppendLine($"{e.FirtName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Task-14
        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectToDelete = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(projectToDelete);

            var project = context.Projects.Find(2);

            context.SaveChanges();

            var projects = context.Projects
                .Select(p => p.Name)
                .Take(10)
                .ToArray();

            return string.Join(Environment.NewLine, projects);
        }

        //Task-15
        public static string RemoveTown(SoftUniContext context)
        {
            var employee = context.Employees
                .Where(e => e.Address!.Town!.Name == "Seattle")
                .ToArray();

            foreach (var e in employee)
            {
                e.AddressId = null;
            }

            var addresesToRemove = context.Addresses
                .Where(a => a.Town!.Name == "Seattle")
                .ToArray();

            context.Addresses.RemoveRange(addresesToRemove);

            var townToRemove = context.Towns
                .Where(t => t.Name == "Seattle");

            context.Towns.RemoveRange(townToRemove);

            context.SaveChanges();

            return $"{addresesToRemove.Count()} addresses in Seattle were deleted";
        }
    }
}