using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Models
{
    public class BranchOffice
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public ICollection<Account> Hesaplar { get; set; }
        [Column("ad")]
        public string Name { get; set; }
        [Column("il")]
        public string City { get; set; }
        [Column("ilce")]
        public string District { get; set; }
    }
}
