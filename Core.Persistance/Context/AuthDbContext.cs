using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistance.Context;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }
}