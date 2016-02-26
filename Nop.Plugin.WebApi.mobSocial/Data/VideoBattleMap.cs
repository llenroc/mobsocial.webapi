﻿using System.Data.Entity.ModelConfiguration;
using Mob.Core.Data;
using Nop.Plugin.WebApi.MobSocial.Domain;

namespace Nop.Plugin.WebApi.MobSocial.Data
{
    public class VideoBattleMap : BaseMobEntityTypeConfiguration<VideoBattle>
    {
        public VideoBattleMap()
        {
            Property(x => x.ChallengerId);
            Property(x => x.Name);
            Property(x => x.Description);
            Property(x => x.VotingStartDate).HasColumnType("datetime2");
            Property(x => x.VotingEndDate).HasColumnType("datetime2");

            Property(x => x.VideoBattleType);
            Property(x => x.VideoBattleVoteType);
            Property(x => x.VideoBattleStatus);

            Property(x => x.MaximumParticipantCount);

            Property(x => x.CoverImageId).IsOptional();

            Property(x => x.IsVotingPayable);
            Property(x => x.CanVoterIncreaseVotingCharge);
            Property(x => x.MinimumVotingCharge);

            Property(x => x.IsSponsorshipSupported);
            Property(x => x.SponsoredCashDistributionType);
            Property(x => x.MinimumSponsorshipAmount);
        }
    }
}
