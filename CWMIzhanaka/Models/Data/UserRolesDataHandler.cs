using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CWMIzhanaka.Models.Data
{
    [Table("UserRoles")]
    public class UserRolesDataHandler
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual AccountsDataHandler Accounts { get; set; }

        [ForeignKey("RoleId")]
        public virtual RolesDataHandler Role { get; set; }

    }
}
