using University.Data;
using University.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace University.Data;

public static class DbInitializer
{
    public static void Initialize(SchoolContext context)
    {
        context.Database.EnsureCreated();
        // Look for any students.
        if (context.Students.Any())
        {
            return;   // DB has been seeded
        }
        var students = new Student[]
        {
            new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2019-09-01")},
            new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2017-09-01")},
            new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2018-09-01")},
            new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2017-09-01")},
            new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2017-09-01")},
            new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2016-09-01")},
            new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2018-09-01")},
            new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2019-09-01")}
        };
        context.Students.AddRange(students);
        context.SaveChanges();
        var courses = new Course[]
        {
            new Course{CourseID=1050,Title="Chemistry",Credits=3},
            new Course{CourseID=4022,Title="Microeconomics",Credits=3},
            new Course{CourseID=4041,Title="Macroeconomics",Credits=3},
            new Course{CourseID=1045,Title="Calculus",Credits=4},
            new Course{CourseID=3141,Title="Trigonometry",Credits=4},
            new Course{CourseID=2021,Title="Composition",Credits=3},
            new Course{CourseID=2042,Title="Literature",Credits=4}
        };
        context.Courses.AddRange(courses);
        context.SaveChanges();
        var enrollments = new Enrollment[]
        {
            new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
            new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
            new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},
            new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
            new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
            new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},
            new Enrollment{StudentID=3,CourseID=1050},
            new Enrollment{StudentID=4,CourseID=1050},
            new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},
            new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},
            new Enrollment{StudentID=6,CourseID=1045},
            new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},
        };
        context.Enrollments.AddRange(enrollments);
            var roles = new IdentityRole[] {
            new IdentityRole{Id="1", Name="Administrator"},
            new IdentityRole{Id="2", Name="Manager"},
            new IdentityRole{Id="3", Name="Staff"}
        };
        foreach (IdentityRole r in roles)
        {
            context.Roles.Add(r);
        }
        var user = new ApplicationUser
        {
            FirstName = "Bob",
            LastName = "Dilon",
            City = "Ljubljana",
            Email = "bob@example.com",
            NormalizedEmail = "XXXX@EXAMPLE.COM",
            UserName = "bob@example.com",
            NormalizedUserName = "bob@example.com",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };
        if (!context.Users.Any(u => u.UserName == user.UserName))
        {
            var password = new PasswordHasher<ApplicationUser>();
            var hashed = password.HashPassword(user,"Testni123!");
            user.PasswordHash = hashed;
            context.Users.Add(user);
            
        }
        context.SaveChanges();
        
        var UserRoles = new IdentityUserRole<string>[]
        {
            new IdentityUserRole<string>{RoleId = roles[0].Id, UserId=user.Id},
            new IdentityUserRole<string>{RoleId = roles[1].Id, UserId=user.Id},
        };
        foreach (IdentityUserRole<string> r in UserRoles)
        {
            context.UserRoles.Add(r);
        }
        context.SaveChanges();
    }
}
