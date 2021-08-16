using BankAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Data
{
    public class DataContext:DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<BranchOffice> BranchOffices { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
