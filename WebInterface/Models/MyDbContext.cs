using System;
using Microsoft.EntityFrameworkCore;
using WebInterface.Models;
using WebInterface.Controllers;

namespace WebInterface.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}