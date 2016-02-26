﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mob.Core;
using Mob.Core.Domain;
using Nop.Core;
using Nop.Core.Domain.Seo;
using Nop.Plugin.WebApi.MobSocial.Enums;

namespace Nop.Plugin.WebApi.MobSocial.Domain
{
    public class VideoBattle : BaseMobEntity, ISlugSupported, INameSupported, IFollowSupported
    {
        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public int ChallengerId { get; set; }

        public DateTime VotingStartDate { get; set; }

        public DateTime VotingEndDate { get; set; }

        public VideoBattleType VideoBattleType { get; set; }

        public VideoBattleStatus VideoBattleStatus { get; set; }

        public VideoBattleVoteType VideoBattleVoteType { get; set; }

        public int MaximumParticipantCount { get; set; }

        public bool IsVotingPayable { get; set; }

        public decimal MinimumVotingCharge { get; set; }

        public bool CanVoterIncreaseVotingCharge { get; set; }

        public decimal ParticipantPercentagePerVote { get; set; }

        public IList<VideoBattlePrize> Prizes { get; set; }

        public int? CoverImageId { get; set; }

        public bool IsSponsorshipSupported { get; set; }

        public decimal MinimumSponsorshipAmount { get; set; }

        public SponsoredCashDistributionType SponsoredCashDistributionType { get; set; }
    }
}
