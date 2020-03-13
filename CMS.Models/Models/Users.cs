using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    [Table("Users", Schema = "auth")]
    public class Users
    {
        [Required(AllowEmptyStrings = true)]
        [MaxLength(5)]
        public string PasswordSalt { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int UserId { get; set; }

        public int? LoaiTaiKhoanID { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(50)]
        [Index("IX_Users_UserName", IsUnique = true, Order = 0)]
        public string UserName { get; set; }

        [Required]
        public bool Active { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }

        public DateTime? CreatedTime { get; set; }

        public DateTime? LastLogin { get; set; }

        [MaxLength(2)]
        public string Lang { get; set; }

        [ForeignKey("LoaiTaiKhoanID")]
        public virtual LoaiTaiKhoan LoaiTaiKhoan { get; set; }

    }
}
