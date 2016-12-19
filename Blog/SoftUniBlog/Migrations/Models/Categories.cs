using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SoftUniBlog.Migrations.Models;
using System.Linq;
using System.Web;
using SoftUniBlog.Models;

namespace SoftUniBlog.Migrations.Models
{
    public class Categories
    {
        private ICollection<Post> posts;
        public Categories()
        {
            this.posts = new HashSet<Post>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [StringLength(20)]
        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}