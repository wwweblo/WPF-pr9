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

        [Required(ErrorMessage = "Role is required")]
        [StringLength(50)]
        public string role { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50)]
        public string name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1,100)]
        public int age { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(50)]
        public string status { get; set; }

        [StringLength(50, ErrorMessage = "Login should be at most 50 characters long")]
        public string login { get; set; }

        [StringLength(50, ErrorMessage = "Password should be at most 50 characters long")]
        public string password { get; set; }
    }
}
