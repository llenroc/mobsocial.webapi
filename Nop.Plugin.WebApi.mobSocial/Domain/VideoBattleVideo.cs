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
    public class VideoBattleVideo : BaseMobEntity
    {
        public string VideoPath { get; set; }

        public string MimeType { get; set; }

        public int ParticipantId { get; set; }

        public int VideoBattleId { get; set; }

        public VideoStatus VideoStatus { get; set; }

        public DateTime DateUploaded { get; set; }

        public virtual VideoBattle VideoBattle { get; set; }

        public string ThumbnailPath { get; set; }
    }
}
