using BankAPI.Data;
using BankAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly DataContext context;
        public AccountsController(DataContext context)
        {
            this.context = context;
        }
        static int NumDigit(long i)
        {
            int count = 0;
            while (i != 0)
            {
                i /= 10;
                count++;
            }
            return count;
        }
        [HttpGet("[action]")]
        public async Task<IEnumerable<Account>> AccountsGet()
        {
            return await context.Accounts.ToListAsync();
        }
        [HttpGet("[action]/tc")]
        public async Task<Account> TcGet(long tc)
        {
            if (await context.Accounts.AnyAsync(x => x.TcNum == tc) == true && NumDigit(tc)==11)
            {
                return await context.Accounts.FirstOrDefaultAsync(x => x.TcNum == tc);
            }
            else return null;         

        }
        [HttpGet("[action]")]
        public async Task<IEnumerable<Account>> BranchandAccounts()
        {
            var hesaplar = await context.Accounts.Include(i => i.Sube).Select(x => new Account()
            {
                Id=x.Id,
                TcNum=x.TcNum,
                Name = x.Name,
                LastName = x.LastName,
                Bakiye=x.Bakiye,
                DateOfBirth=x.DateOfBirth,                
                TelNum=x.TelNum,
                SubeId = x.SubeId,
                Sube =new BranchOffice()
                {
                    Id=x.Id,
                    Name=x.Sube.Name,
                    City=x.Sube.City,
                    District=x.Sube.District
                }
            }).ToListAsync();
          
            return hesaplar;
        }

        [HttpGet("[action]/{ct}")]
        public async Task<IEnumerable<Account>> EqualCityAccount(string ct)
        {
            var cityAcc = await context.Accounts.Include(i => i.Sube).Where( i => i.Sube.City == ct).ToListAsync();
            foreach(var hesap in cityAcc)
            {
                hesap.Sube.Hesaplar = null;
            }
            return cityAcc;

        }

        [HttpPost("[action]")]
        public async Task<string> AccountsPost([FromBody]Account account)
        {
            if (NumDigit(account.TcNum) == 11 && NumDigit(account.TelNum) == 10)
            {
                await context.Accounts.AddAsync(account);
                _ = await context.SaveChangesAsync();
                return "Hesap eklendi!";
            }
            else if (NumDigit(account.TcNum) != 11)
            {
                return "Lütfen TC Kimlik numaranızı doğru giriniz!";
            }
            else if (NumDigit(account.TelNum) != 10)
            {
                return "Lütfen telefon numaranızı doğru giriniz!";
            }
            else return "Lütfen TC Kimlik numaranızı ve telefon numaranızı doğru giriniz!";            
        }
        [HttpPut("[action]")]
        public async Task<Account> AccountsPut(Account account)
        {
            var editAcc =await context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id);
            editAcc.TcNum = account.TcNum;
            editAcc.Name = account.Name;
            editAcc.LastName = account.LastName;
            editAcc.TelNum = account.TelNum;
            editAcc.Bakiye = account.Bakiye;
            editAcc.DateOfBirth = account.DateOfBirth;
            _=await context.SaveChangesAsync();
            return account;
        }
        [HttpPut("[action]/{tc_nerden}/{tc_nereye}/{transfer}")]
        public async Task<Tuple<bool ,string>> HesaplarArasiTransfer(long tc_nerden,long tc_nereye,decimal transfer)
        {
            bool Tc1 = await context.Accounts.AnyAsync(x => x.TcNum == tc_nerden);
            bool Tc2 = await context.Accounts.AnyAsync(x => x.TcNum == tc_nereye);
            if (Tc1 == true && Tc2 == true)
            {
                var acc1 = await context.Accounts.FirstOrDefaultAsync(x => x.TcNum == tc_nerden);
                var acc2 = await context.Accounts.FirstOrDefaultAsync(x => x.TcNum == tc_nereye);
                if (acc1.TcNum == acc2.TcNum)
                {
                    return Tuple.Create(false, "Aynı hesaba para transferi yapılamaz");
                }
                else if (acc1.Bakiye < transfer)
                {
                    return Tuple.Create(false, "Bakiye Yetersiz!");
                }
                else if (transfer <= 0)
                {
                    return Tuple.Create(false, "Geçerli para miktarı giriniz!");
                }
                else
                {
                    acc1.Bakiye -= transfer;
                    acc2.Bakiye += transfer;
                    _= await context.SaveChangesAsync();
                    return Tuple.Create(true, "Başarılı!");
                }
            }
            else
            {
                return Tuple.Create(false, "Geçerli TC Numarası giriniz!");
            }
           
        }
        [HttpDelete("[action]/{tc}")]
        public async Task<string> AccountsDelete(long tc)
        {
            if (await context.Accounts.AnyAsync(x => x.TcNum == tc) == true)
            {
                var deleteAcc = await context.Accounts.FirstOrDefaultAsync(x => x.TcNum == tc);
                context.Accounts.Remove(deleteAcc);
                _ = await context.SaveChangesAsync();
                return "Hesap silindi!";
            }
            else return "Geçerli TC kimlik numarası giriniz!";
        }
    }
}
