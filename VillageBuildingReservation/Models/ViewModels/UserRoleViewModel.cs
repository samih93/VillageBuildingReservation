﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VillageBuildingReservation.Models.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}