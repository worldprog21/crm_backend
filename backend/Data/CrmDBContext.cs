using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class CrmDBContext : DbContext
{
    public CrmDBContext(DbContextOptions<CrmDBContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Lead> Leads { get; set; }
    public DbSet<Opportunity> Opportunities { get; set; }
    public DbSet<Activity> Activities { get; set; }

}
