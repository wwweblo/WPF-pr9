namespace WpfApp3
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employee")]
    public partial class Employee
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string role { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [Required]
        [StringLength(50)]
        public string email { get; set; }

        public int age { get; set; }

        [Required]
        [StringLength(50)]
        public string status { get; set; }

        [StringLength(50)]
        public string login { get; set; }

        [StringLength(50)]
        public string password { get; set; }
    }
}
