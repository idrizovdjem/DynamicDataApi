﻿using System;
using Microsoft.AspNetCore.Identity;

namespace DynamicData.Web.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
