﻿using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.ForgetPasswordDTO
{
    public class VerifyResetCodeDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; }
    }
}
