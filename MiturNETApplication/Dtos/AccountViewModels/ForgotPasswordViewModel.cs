﻿namespace MiturNetApplication.Dtos.AccountViewModels;
public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
