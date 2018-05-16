using System;
using Microsoft.EntityFrameworkCore;
using UserCustimizationMenu.Models;
using UserCustimizationMenu.Controllers;

namespace UserCustimizationMenu.Models
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