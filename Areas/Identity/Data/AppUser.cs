using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Task4.Areas.Identity.Data.Enum;

namespace Task4.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    [DataType(DataType.DateTime)]
    public DateTime? LastLogInTime { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime RegistrationTime { get; set; }

    public UserStatus Status { get; set; }

    [NotMapped]
    public bool CheckBox { get; set; }
}

