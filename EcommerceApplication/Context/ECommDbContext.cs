﻿using EcommerceApplication.Models;
using Microsoft.EntityFrameworkCore;
using EcommerceApplication.Models.ViewModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EcommerceApplication.Context
{
	public class ECommDbContext : IdentityDbContext<ApplicationUser>
	{
		public ECommDbContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<Category> categories { get; set; }	
		public DbSet<Product> products { get; set; }	
		public DbSet<EcommerceApplication.Models.Registration> Registration { get; set; } = default!;
		//public DbSet<EcommerceApplication.Models.Registration> Registration { get; set; } = default!;
		//public DbSet<Registration> registrations { get; set; }	
	}
}
