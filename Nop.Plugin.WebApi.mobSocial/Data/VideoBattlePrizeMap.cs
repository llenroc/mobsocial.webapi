﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mob.Core.Data;
using Nop.Plugin.WebApi.MobSocial.Domain;

namespace Nop.Plugin.WebApi.MobSocial.Data
{
    public class VideoBattlePrizeMap : BaseMobEntityTypeConfiguration<VideoBattlePrize>
    {
        public VideoBattlePrizeMap()
        {
            Property(x => x.Description);
            Property(x => x.PrizeAmount);
            Property(x => x.PrizeOther);
            Property(x => x.PrizePercentage);
            Property(x => x.PrizeProductId);
            Property(x => x.PrizeType);
            Property(x => x.VideoBattleId);
            Property(x => x.WinnerId);
            Property(x => x.WinnerPosition);
            Property(x => x.IsSponsored);
            Property(x => x.SponsorCustomerId);
        }
    }
}
