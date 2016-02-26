﻿using System.Collections.Generic;
using Nop.Plugin.WebApi.MobSocial.Domain;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.WebApi.MobSocial.Models
{
    public class CustomerAlbumModel : BaseNopModel
    {

        public CustomerAlbumModel()
        {
            Pictures = new List<CustomerAlbumPictureModel>();
        }
        public bool IsCurrentUsersProfile { get; set; }
        public int AlbumId { get; set; } 
        public string AlbumName { get; set; }

        public string CustomerFullName { get; set; }

        public List<CustomerAlbumPictureModel> Pictures { get; set; }

        
    }
}


