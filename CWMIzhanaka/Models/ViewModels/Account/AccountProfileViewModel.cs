﻿using CWMIzhanaka.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CWMIzhanaka.Models.ViewModels.Account
{
    public class AccountProfileViewModel
    {

        public AccountProfileViewModel() { }

        public  AccountProfileViewModel(AccountsDataHandler adh)
        {
            UserId = adh.UserId;
            Username = adh.Username;
            Firstname = adh.Firstname;
            Lastname = adh.Lastname;
            EmailAddress = adh.EmailAddress;
            Password = adh.Password;
        }
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}