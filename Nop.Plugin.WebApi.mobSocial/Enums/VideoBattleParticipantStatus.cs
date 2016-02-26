﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.WebApi.MobSocial.Enums
{
    public enum VideoBattleParticipantStatus
    {
        NotChallenged = 0,
        Challenged = 10,
        ChallengeAccepted = 20,
        ChallengeDenied = 30,
        ChallengeCancelled = 40,
        SignedUp = 50,
        Disqualified = 60
    }
}
