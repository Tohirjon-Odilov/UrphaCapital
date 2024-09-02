﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UrphaCapital.Application.Abstractions;
using UrphaCapital.Application.HasherServices;
using UrphaCapital.Domain.Entities;
using UrphaCapital.Domain.Entities.Auth;

namespace UrphaCapital.Infrastructure.Persistanse
{
    public class UrphaCapitalDbContext : DbContext, IApplicationDbContext
    {
        public UrphaCapitalDbContext(DbContextOptions<UrphaCapitalDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Homeworks> Homeworks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Homeworks>()
    .HasOne(h => h.Lesson)
    .WithMany()
    .HasForeignKey(h => h.LessonId);


            var password = "Admin01!";
            var salt = Guid.NewGuid().ToString();
            var hashedPass = Encrypt(password, salt);
            modelBuilder.Entity<Admin>().HasData(new Admin()
            {
                Id = 1,
                Name = "Ozodali",
                Email = "admin@gmail.com",
                PhoneNumber = "+998934013443",
                Role = "SuperAdmin",
                PasswordHash = "Admin01!",
                Salt = Guid.NewGuid().ToString()
            });

        }

        async ValueTask<int> IApplicationDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public string Encrypt(string password, string salt)
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                iterations: 1000,
                hashAlgorithm: HashAlgorithmName.SHA256))
            {
                var bytes = algorithm.GetBytes(32);
                return Convert.ToBase64String(bytes);
            }
        }

    }
}
