using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Models
{
    public class Account
    {

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("subeId")]
        public int SubeId { get; set; }
        [ForeignKey(nameof(SubeId))]
        public BranchOffice Sube { get; set; }        
        [Column("tcNum")]
        public long TcNum { get; set; }
        [Column("ad")]
        public string Name { get; set; }
        [Column("soyad")]
        public string LastName { get; set; }
        [Column("bakiye")]
        public decimal Bakiye { get; set; }
        [Column("telefon")]
        public long TelNum { get; set; }
        [Column("dogumGunu")]
        public DateTime DateOfBirth { get; set; }
    }
}
