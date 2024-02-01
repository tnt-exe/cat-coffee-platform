using BusinessObject.Model;
using DAO.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAO.Context.ApplicationDbContext;

namespace DAO.Helper
{
    public class CustomUserStore : UserStore<User, IdentityRole<Guid>, ApplicationDbContext ,Guid>
    {
        public CustomUserStore(IDesignTimeDbContextFactory<ApplicationDbContext> factory,IdentityErrorDescriber describer = null) : base(factory.CreateDbContext(new string[0]), describer)
        {
        }
    }
}
