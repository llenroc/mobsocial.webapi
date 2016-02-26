﻿using System.Data.Entity.ModelConfiguration;
using Mob.Core.Data;
using Nop.Plugin.WebApi.MobSocial.Domain;

namespace Nop.Plugin.WebApi.MobSocial.Data
{
    //todo: make into social network plugin and design social network plugin API
    public class SkateMoveMap : BaseMobEntityTypeConfiguration<SkateMove>
    {

        public SkateMoveMap()
        {
            //Map the additional properties
            Property(m => m.SkatingTypeId);
            Property(m => m.Name);
            Property(m => m.Description);
            Property(m => m.BadgeImageUrl);

        }

    }
}
