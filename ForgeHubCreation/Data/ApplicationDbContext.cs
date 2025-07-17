﻿using ForgeHubCreation.Model;
using Microsoft.EntityFrameworkCore;

namespace ForgeHubCreation.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
    }
}
