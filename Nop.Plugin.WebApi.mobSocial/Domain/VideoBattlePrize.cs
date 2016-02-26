﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mob.Core.Domain;
using Nop.Plugin.WebApi.MobSocial.Enums;

namespace Nop.Plugin.WebApi.MobSocial.Domain
{
    public class VideoBattlePrize: BaseMobEntity
    {
        public int VideoBattleId { get; set; }

        public int WinnerPosition { get; set; }

        public VideoBattlePrizeType PrizeType { get; set; }

        public string Description { get; set; }

        public decimal PrizePercentage { get; set; }

        public decimal PrizeAmount { get; set; }

        public int PrizeProductId { get; set; }

        public string PrizeOther { get; set; }

        public int WinnerId { get; set; }

        public bool IsSponsored { get; set; }

        public int SponsorCustomerId { get; set; }

    }
}
