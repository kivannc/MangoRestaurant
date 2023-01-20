using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

       
}