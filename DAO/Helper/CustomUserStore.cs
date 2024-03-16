using BusinessObject.Model;
using DAO.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAO.Helper
{
    public class CustomUserStore : UserStore<User, IdentityRole<Guid>, ApplicationDbContext, Guid>
    {
        public CustomUserStore(IDesignTimeDbContextFactory<ApplicationDbContext> factory, IdentityErrorDescriber describer = null) : base(factory.CreateDbContext(new string[0]), describer)
        {
        }
    }
}
