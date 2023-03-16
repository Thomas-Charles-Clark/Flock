using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.Mvc;

namespace Flock.Models
{
    public class Designer
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// Checks if user with given password exists in the database
        /// </summary>
        /// <param name="_username">User name</param>
        /// <param name="_password">User password</param>
        /// <returns>True if user exist and password is correct</returns>
        public bool IsValid(string _username, string _password)
        {
            using (FlockDataContext db = new FlockDataContext())
            {
                Admin admin = null;//ad.Admins.SingleOrDefault(p => (p.Email == _username && p.Password == Helper._password));

                if (admin != null)
                {
                    // valid;
                    return true;
                }
                else
                {
                    // invalid
                    return false;
                }
            }
        }
    }
}