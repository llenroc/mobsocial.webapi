﻿using System;
using Nop.Core;
using System.Web.Script.Serialization;
using Mob.Core.Domain;


namespace Nop.Plugin.WebApi.MobSocial.Domain
{
    public class CustomerAlbumPicture : BaseMobEntity
    {
        public int CustomerAlbumId { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Caption { get; set; }
        public int DisplayOrder { get; set; }
        public int LikeCount { get; set; }


        [ScriptIgnore]
        public virtual CustomerAlbum Album { get; set; }

        
    }


}