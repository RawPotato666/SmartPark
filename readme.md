# Vaje IS

## Vaja 03

Na posnetku je podrobno razložena izdelava aplikacije iz ASP.NET [dokumentacije](https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-8.0).

Na vaji se spoznamo s pristopom Model–view–controller (MVC) za izdelavo spletnih aplikacij. Spletno aplikacijo povežemo z relacijsko podatkovno bazo s pomočjo ogrodja EntityFramework (ORM). Na naslednji vaji bomo izdelano aplikacijo nadgrajevali.

Programska oprema potrebna za sledenje vaji:

- Docker Desktop za zagon strežnika Microsoft SQL Server, alternativa orodje OrbStack
- docker image Microsoft SQL Server
- dotnet-ef in dotnet-asp-codegenerator

Code snippets:
```csharp
# zagon MS SQL Server docker containerja, macOS/Linux
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=yourStrong(!)Password' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-CU15-GDR1-ubuntu-22.04

# zagon MS SQL Server docker containerja, Windows
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-CU15-GDR1-ubuntu-22.04

# zagon Azure SQL Edge (IoT verzija SQL Serverja), ARM, Apple Silicon
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=yourStrong(!)Password' -p 1433:1433 --name azuresqledge -d mcr.microsoft.com/azure-sql-edge

# namestitev dotnet-ef
dotnet tool install --global dotnet-ef --version "8.*"

# namestitev dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-aspnet-codegenerator --version "8.*"

# package references v .csproj
dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v "8.*"
dotnet add package Microsoft.EntityFrameworkCore.Design -v "8.*"
dotnet add package Microsoft.EntityFrameworkCore.Tools -v "8.*"
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design -v "8.*"

# db connection string v appsettings.json
"ConnectionStrings": {
"SchoolContext": "Server=localhost;Database=University;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=true"
}

##### Models/Student.cs
namespace University.Models;
using System;
using System.Collections.Generic;

public class Student
{
    public int ID { get; set; }
    public string LastName { get; set; }
    public string FirstMidName { get; set; }
    public DateTime EnrollmentDate { get; set; }

}
#####

##### Data/SchoolContext.cs
namespace University.Data;
using University.Models;

using Microsoft.EntityFrameworkCore;

public class SchoolContext : DbContext
{

public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
{

}

public DbSet<Student> Students { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
modelBuilder.Entity<Student>().ToTable("Student");
}

}

#####

##### Program.cs
using University.Data;
using Microsoft.EntityFrameworkCore;

...

builder.Services.AddDbContext<SchoolContext>(options =>

options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));

...

#####


# migrations and db update
dotnet ef migrations add Initial
dotnet ef database update

# generate MVC
# controller and views based on Student.cs 
dotnet aspnet-codegenerator controller -name StudentsController -m Student -dc University.Data.SchoolContext -udl -outDir Controllers

# controller and views based on Course.cs 
dotnet aspnet-codegenerator controller -name CoursesController -m Course -dc University.Data.SchoolContext -udl -outDir Controllers

```

## Vaja 04

Cilj naloge je razširiti obstoječo MVC frontend aplikacijo z uporabniki in avtentikacijo. Nadgradili bomo rešitev iz prejšnjih vaj. Najprej uredimo povezavo do podatkovne baze. Objavljen je posnetek vaje.

1. V rešitev namestimo knjižnico Microsoft.AspNetCore.Identity.EntityFrameworkCore in Microsoft.AspNetCore.Identity.UI aktualne verzije. Uporabimo NuGet package manager (Package manager, .NET cli ali PackageReference v .csproj).

```csharp
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version "8.*"
dotnet add package Microsoft.AspNetCore.Identity.UI --version "8.*"
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore --version "8.*"
```

2. V Models dodamo nov razred ApplicationUser.cs, ki deduje razred IdentityUser ( iz knjižnice Microsoft.AspNetCore.Identity). Razredu dodamo atribute FirstName, LastName, City.

```csharp
using Microsoft.AspNetCore.Identity;

namespace University.Models;

public class ApplicationUser : IdentityUser
{
     public string? FirstName { get; set; }
     public string? LastName { get; set; }
     public string? City { get; set; }
}
```

3. V datoteki SchoolContext.cs dodamo `using Microsoft.AspNetCore.Identity.EntityFrameworkCore;` in spremenimo dedovanje (`: IdentityDbContext<ApplicationUser>`) in dodamo vrstico `base.OnModelCreating(modelBuilder);` v `OnModelCreating()`.

4. Ustvarimo migracije in jih izvedemo na bazo.

```csharp
dotnet ef migrations add AppUser
# izbrišemo obstoječo bazo s podatki in nato ustvarimo novo
dotnet ef database drop
dotnet ef database update
```

5. Zaženemo generator kode za Identity:
```csharp
dotnet aspnet-codegenerator identity -dc University.Data.SchoolContext --generateLayout

#ali (samo določene view-e)
dotnet aspnet-codegenerator identity -dc University.Data.SchoolContext -fi "Account.Register;Account.Login;Account.Logout;Account.RegisterConfirmation" --generateLayout
```

6. Zamenjaj v Program.cs:
```
// uvozi
using Microsoft.AspNetCore.Identity;
using University.Models;
// nastavi spremenljivko connectionString za .useSqlServer(connectionString)
var connectionString = builder.Configuration.GetConnectionString("SchoolContext");

// nadomesti stari .AddDbContext
builder.Services.AddDbContext<SchoolContext>(options =>
            options.UseSqlServer(connectionString));

// prilagodi RequireConfirmedAccount = false in .AddRoles<IdentityRole>()
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SchoolContext>();

// dodaj app.MapRazorPages(); (npr. za app.useAuthentication())
app.MapRazorPages();
```

7. Dodamo `<partial name="_LoginPartial" />` v /Views/Shared/_Layout.cshtml (na koncu znotraj elementa `<div class="navbar-collapse..."></div>)`.

8. Dodamo avtorizacijo na posamezne metode v StudentsController.cs in nad celoten CourseController. `[Authorize]` in `using Microsoft.AspNetCore.Authorization;`

9. Razširimo Course.cs z lastnostmi Owner, DateCreated, DateEdited.

10. Posodobimo bazo.

```csharp
dotnet ef migrations add ExtendCourse
dotnet ef database update
```

11. Ob kreiranju novega predmeta v Course.OwnerId zapišemo id trenutno prijavljenega uporabnika.

12. V tabelo AspNetRoles dodaj 3 različne vloge (Administrator, Manager, Staff). Vloge dodeliš uporabniku z zapisi v tabeli AspNetUserRoles (zapis: id vloge | id uporabnika).

13. Omeji dostop do operacij v CourseController.cs s pomočjo `[Authorize(Roles = "Administrator, Manager, Staff")]`.

14. Dopolni `DbInitializer.cs` za vnos podatkov o uporabniku, vlogah in povezavi med uporabnikom ter vlogo.

## Vaja 05