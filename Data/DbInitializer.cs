using SmartPark.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SmartPark.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SmartParkContext context)
        {
            context.Database.EnsureCreated();

            // --- SEED USERS (Uporabniki) ---
            if (context.Users.Any())
            {
                return; // DB already seeded
            }

            var users = new User[]
            {
                new User { FirstName = "Janez", LastName = "Novak", Username = "janez.novak" },
                new User { FirstName = "Ana", LastName= "Kovač", Username = "ana.kovac" },
                new User { FirstName = "Miha", LastName = "Horvat", Username = "miha.horvat" },
                new User { FirstName = "Sara", LastName = "Zupan", Username = "sara.zupan" },
                new User { FirstName = "Luka", LastName = "Mali", Username = "luka.mali" },
                new User { FirstName = "Maja", LastName = "Breznik", Username = "maja.breznik" },
                new User { FirstName = "Marko", LastName = "Kralj", Username = "marko.kralj" }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            var administrators = new Administrator[]
            {
                new Administrator { Id = 1, AdminName = "Admin1" },
                new Administrator { Id = 2, AdminName = "Admin2" }
            };
            context.Administrators.AddRange(administrators);
            context.SaveChanges();

            // --- SEED PARKIRIŠČA (Parking lots) ---
            var parkingL = new ParkingLot[]
            {
                new ParkingLot { Location = "Ljubljana - Center", Capacity = 10, DisabledSpots = 2 },
                new ParkingLot { Location = "Ljubljana - BTC", Capacity = 20, DisabledSpots = 4 }
            };
            context.ParkingLots.AddRange(parkingL);
            context.SaveChanges();

            // --- SEED PARKIRNA MESTA (Parking spots) ---
            var parkingSpots = new ParkingSpot[]
            {
                new ParkingSpot { IsDisabled = true, ParkingLotId = parkingL[0].Id },
                new ParkingSpot { IsDisabled = true, ParkingLotId = parkingL[0].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[0].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[0].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[0].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[0].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[0].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[0].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[0].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[0].Id },
                new ParkingSpot { IsDisabled = true, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = true, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = true, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = true, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },
                new ParkingSpot { IsDisabled = false, ParkingLotId = parkingL[1].Id },

            };
            context.ParkingSpots.AddRange(parkingSpots);
            context.SaveChanges();

            // --- SEED REZERVACIJE (Reservations) ---
            var reservations = new Reservation[]
            {
                new Reservation
                {
                    UserId = users[0].Id,
                    ParkingSpotId = parkingSpots[2].Id,
                    Start = DateTime.Now.AddHours(-1),
                    End = DateTime.Now.AddHours(2)
                },
                new Reservation
                {
                    UserId = users[1].Id,
                    ParkingSpotId = parkingSpots[0].Id,
                    Start = DateTime.Now.AddHours(1),
                    End = DateTime.Now.AddHours(3)
                }
            };
            context.Reservations.AddRange(reservations);
            context.SaveChanges();
/*
            // --- SEED ADMINISTRATOR ROLES & USERS (Identity) ---
            var roles = new IdentityRole[]
            {
                new IdentityRole { Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
                new IdentityRole { Name = "Uporabnik", NormalizedName = "UPORABNIK" }
            };
            foreach (var role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role.Name))
                    context.Roles.Add(role);
            }
            context.SaveChanges();

            var adminUser = new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "SmartPark",
                Email = "admin@smartpark.si",
                NormalizedEmail = "ADMIN@SMARTPARK.SI",
                UserName = "admin@smartpark.si",
                NormalizedUserName = "ADMIN@SMARTPARK.SI",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!context.Users.Any(u => u.UserName == adminUser.UserName))
            {
                var passwordHasher = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123!");
                context.Users.Add(adminUser);
                context.SaveChanges();

                var adminRole = context.Roles.First(r => r.Name == "Administrator");
                context.UserRoles.Add(new IdentityUserRole<string>
                {
                    RoleId = adminRole.Id,
                    UserId = adminUser.Id
                });
                context.SaveChanges();
            }
*/
        }
    }
}