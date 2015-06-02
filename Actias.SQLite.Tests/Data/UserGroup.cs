using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Actias.SQLite.Tests.Data
{
    [Table("UserGroups")]
    public class UserGroup
    {
        [Column(Order = 0), Key]
        public int UserId { get; set; }

        [Column(Order = 1),Key]
        public int GroupId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
}
