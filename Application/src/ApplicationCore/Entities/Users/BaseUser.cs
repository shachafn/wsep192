﻿using ApplicationCore.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities.Users
{
   [Table("Users")]
    public class BaseUser : BaseEntity
    {
        [Key]
        [Column(Order =2)]
        public string Username { get; private set; }
        // We only keep the password's hash, you can check if a password is
        // this user's password using the CheckPass function
        [Required(ErrorMessage = "Message is required")]
        private string _passHash;
        [Required(ErrorMessage = "IsAdmin is required")]
        public bool IsAdmin { get; private set; }

        public BaseUser(string username, string password, bool isAdmin)
        {
            Username = username;
            _passHash = GetStringSha256Hash(password);
            IsAdmin = isAdmin;
        }

        /// <summary>
        /// A method that is used to hash the password
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Hashed version of the password</returns>
        private static string GetStringSha256Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

        public bool CheckPass(string password)
        {
            return _passHash.Equals(GetStringSha256Hash(password));
        }
    }
}
