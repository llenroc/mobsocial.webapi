﻿using System;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.WebApi.MobSocial.Models.TeamPage
{
    public class TeamPageModel : BaseNopEntityModel
    {

        public virtual DateTime CreatedOn { get; set; }

        public virtual int CreatedBy { get; set; }

        public virtual DateTime UpdatedOn { get; set; }

        public virtual int UpdatedBy { get; set; }

        public virtual string Description { get; set; }

        public virtual string TeamPictureUrl { get; set; }

        [Required]
        public virtual string Name { get; set; }
    }
}