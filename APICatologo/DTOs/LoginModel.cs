﻿using System.ComponentModel.DataAnnotations;

namespace APICatologo.DTOs
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Username is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]

        public string? Password { get; set; }
    }
}
