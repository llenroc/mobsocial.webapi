﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mob.Core.Domain;
using Nop.Core;

namespace Nop.Plugin.WebApi.MobSocial.Domain
{
    public class VideoBattleView : BaseMobEntity
    {
        public int UserId { get; set; }

        public int VideoBattleVideoId { get; set; }


    }
}
