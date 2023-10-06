using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SystemX_BlazorServerApp.Models
{
    [Table("Users")]
    [Index("UserCode", Name = "UIDX_USERS", IsUnique = true)]
    public partial class User0
    {
        public User0()
        {
            //ClmModulesUsedByUsers = new HashSet<ClmModulesUsedByUser>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }

        //[Column("ULogin")]
        //[StringLength(36)]
        //[Unicode(false)]
        //public string UserCode { get; set; } = null!;

        [Column("Login")]
        [StringLength(20)]
        [Unicode(false)]
        public string Login { get; set; } = null!;

        [Column("Password")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Password { get; set; }

        [Column("IPAddress")]
        [StringLength(30)]
        [Unicode(false)]
        public string IpAddress { get; set; } = null!;

        [Column("LastAccessDateTime", TypeName = "datetimeoffset")]
        public DateTimeOffset LastAccessDateTime { get; set; }

        //public virtual ICollection<ClmModulesUsedByUser> ClmModulesUsedByUsers { get; set; }
    }
}
