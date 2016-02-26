﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mob.Core.Domain;
using Nop.Core;
using Nop.Plugin.WebApi.MobSocial.Enums;

namespace Nop.Plugin.WebApi.MobSocial.Domain
{
    public class VideoBattleParticipant : BaseMobEntity
    {
        public int VideoBattleId { get; set; }

        public int ParticipantId { get; set; }

        public VideoBattleParticipantStatus ParticipantStatus { get; set; }

        public DateTime LastUpdated { get; set; }

        public string Remarks { get; set; }
        
        public virtual VideoBattle VideoBattle { get; set; }

    }
}
