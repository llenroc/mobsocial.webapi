﻿using Nop.Plugin.WebApi.MobSocial.Domain;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.WebApi.MobSocial.Models
{
    public class CustomerProfileModel : BaseNopModel
    {
        public int CustomerId { get; set; }
        public string AboutMe { get; set; }
        public string Website { get; set; }
        public bool IsSelf { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Views { get; set; }
        public int FriendCount { get; set; }
        public bool IsFriend { get; set; }

        public List<CustomerFavoriteSong> FavoriteSongs { get; set; }
    }
}