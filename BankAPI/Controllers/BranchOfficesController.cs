using BankAPI.Data;
using BankAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchOfficesController : ControllerBase
    {
        private readonly DataContext context;
        public BranchOfficesController(DataContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<BranchOffice>> BranchandAccounts()
        {
            return await context.BranchOffices.Include(x=>x.Hesaplar).ToListAsync();
        }

    }
}
