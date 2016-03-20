using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKAnalyzer.DTO;

namespace VKAnalyzer
{
    class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}
