#region 1:M and M:M
//Step-1 Adding packages and using namespaces
//1.Microsoft.EntityFrameworkCore.SqlServer
//2.Microsoft.EntityFrameworkCore.Tools
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("Welcome to EF");

OrganizationDbContext orgDbContext = new OrganizationDbContext();
Console.WriteLine("All Departments");
List<Department> depts = orgDbContext.Departments.ToList();
foreach (var D in depts)
{
    Console.WriteLine($"Did:{D.Did} DName:{D.DName} Description:{D.Description}");
}

Console.WriteLine("All Employees");
List<Employee> emps = orgDbContext.Employees.ToList();
foreach (var E in emps)
{
    Console.WriteLine($"Eid:{E.Eid} Name:{E.FullName} Salary:{E.Salary} Did:{E.Did} DName:{E.Department.DName}");
}

//ORM Tools - EF   

#region Step - 2 Design Entities
#region 1:M Relationship
[Table("Department")]
public class Department
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Did { get; set; }

    [Column("DepartmentName")]
    public string? DName { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? Description { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedOn { get; set; }
    public List<Employee>? Employees { get; set; }
}

[Table("Employee")]
public class Employee
{
    [Key]
    public int Eid { get; set; }

    [Required]
    public string? FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string? LastName { get; set; }

    [NotMapped]
    public string? FullName => (FirstName + " " + LastName);
    public string? Gender { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [EmailAddress]
    [Compare("Email")]
    [NotMapped]
    public string? ConfirmEmail { get; set; }

    [Column(TypeName = "Date")]
    public DateTime DOB { get; set; }

    [Range(10000, 100000)]
    public double Salary { get; set; }

    [Url]
    public string? LinkedInProfileLink { get; set; }

    [ForeignKey("Department")]
    public int? Did { get; set; }
    public Department? Department { get; set; }
}

#endregion

#region M:M Relationship
[Table("Student")]
public class Student
{
    [Key]
    public int Sid { get; set; }
    public string? Name { get; set; }
    public List<StudentCourse>? StudentCourses { get; set; }
}

[Table("Course")]
public class Course
{
    [Key]
    public int Cid { get; set; }
    public string? CName { get; set; }
    public List<StudentCourse>? StudentCourses { get; set; }
}

[Table("StudentCourse")]
public class StudentCourse
{
    [Key]
    public int SCId { get; set; }

    [ForeignKey("Student")]
    public int Sid { get; set; }

    public Student? Student { get; set; }

    [ForeignKey("Course")]
    public int Cid { get; set; }

    public Course? Course { get; set; }
}
#endregion
#endregion

//Step - 3 Creating DbContext Class
public class OrganizationDbContext : DbContext
{
    //Step - 4 Connecting to Db
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Server=localhost;Database=OrgEFDb_2;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    //Step - 5 Creating DbSets
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }


    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }
}

//Step - 6 Generating Database and Tables
//Run 2 commands
//1. add-migration FirstMigrationFile
//2. update-database 
#endregion

#region Project_Module Design
[Table("Project")]
public class Project
{
    [Key]
    public int Pid { get; set; }
    public string ProjectName { get; set; }

    public List<Module> Modules { get; set; }
}

[Table("Module")]
public class Module
{
    [Key]
    public int Mid { get; set; }
    public string ModuleName { get; set; }

    [ForeignKey("ProjectNvg")]
    public int Pid { get; set; }
    public virtual Project ProjectNvg { get; set; }

    public List<Task> Tasks { get; set; }
}

[Table("Task")]
public class Task
{
    [Key]
    public int Tid { get; set; }
    public string TaskName { get; set; }

    [ForeignKey("ModuleNvg")]
    public int Mid { get; set; }
    public virtual Module ModuleNvg { get; set; }

    [ForeignKey("EmployeeNvg")]
    public int Eid { get; set; }
    public virtual Employee EmployeeNvg { get; set; }
}

#endregion