using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Actias.SQLite.Tests.Data
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Index("IX_User_Username")]
        public string Username { get; set; }

        public virtual IEnumerable<UserGroup> Groups { get; set; }
    }
}
