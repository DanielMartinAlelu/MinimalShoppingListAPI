using Microsoft.EntityFrameworkCore;

namespace MinimalShoppingListAPI
{
    //Ceate our DataBase (Db injection)
    public class ApiDbContext : DbContext
    {
        public DbSet<Grocery> Groceries => Set<Grocery>();
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
    }
}
