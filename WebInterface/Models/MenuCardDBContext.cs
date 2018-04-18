using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class MenuCardDBContext : DbContext
    {
        public MenuCardDBContext(DbContextOptions<MenuCardDBContext> options) : base(options)
        {

        }

        public DbSet<Guest> Guests { get; set; }
    }
}
