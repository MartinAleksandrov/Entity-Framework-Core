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
            var result9 = GetEmployee147(context);
            Console.WriteLine(result9);
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
    }
}