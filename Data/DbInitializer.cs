using SmartPark.Models;
using SmartPark.Data;
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
                new User { FirstName = "Janez", LastName = "Novak" },
                new User { FirstName = "Ana", LastName= "Kovač" },
                new User { FirstName = "Miha", LastName = "Horvat" },
                new User { FirstName = "Sara", LastName = "Zupan" },
                new User { FirstName = "Luka", LastName = "Mali" },
                new User { FirstName = "Maja", LastName = "Breznik" },
                new User { FirstName = "Marko", LastName = "Kralj"  }
            };
            context.Users.AddRange(users);
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
            ParkingSpot[] parkingSpots;
            var generatedSpots = new System.Collections.Generic.List<ParkingSpot>();
            foreach (var lot in parkingL)
            {
                for (int i = 1; i <= lot.Capacity; i++)
                {
                    generatedSpots.Add(new ParkingSpot
                    {
                        ParkingLotId = lot.Id,
                        DisplayId = i, // per-lot display number
                        IsDisabled = i <= lot.DisabledSpots,
                        IsOccupied = false
                    });
                }
            }
            parkingSpots = generatedSpots.ToArray();
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

        
            var roles = new IdentityRole[] {
            new IdentityRole{Id="1", Name="Administrator"},
            new IdentityRole{Id="2", Name="Manager"},
            };
            foreach (IdentityRole r in roles)
            {
                context.Roles.Add(r);
            }
                var user = new User
                {
                    FirstName = "Bob",
                    LastName = "Dilon",
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
                    var password = new PasswordHasher<User>();
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
    }
