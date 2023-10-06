﻿using System;
using BlazorApp.Data.Entities;

namespace BlazorApp.Data.WebAPI;
public class UserWithToken : User
{
    
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public UserWithToken(User user)
    {
        this.UserId = user.UserId;
        this.EmailAddress = user.EmailAddress;            
        this.FirstName = user.FirstName;
        this.MiddleName = user.MiddleName;
        this.LastName = user.LastName;
        this.PubId = user.PubId;
        this.HireDate = user.HireDate;

        this.Role = user.Role;
    }
}
