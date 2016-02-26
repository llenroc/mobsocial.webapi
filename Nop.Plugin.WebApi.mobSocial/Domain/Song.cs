﻿using Mob.Core;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Seo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mob.Core.Domain;

namespace Nop.Plugin.WebApi.MobSocial.Domain
{
    public class Song : BaseMobEntity, ISlugSupported, INameSupported
    {
        public int PageOwnerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RemoteEntityId { get; set; }
        public string RemoteSourceName { get; set; }
        public string PreviewUrl { get; set; }
        public string TrackId { get; set; }
        public string RemoteArtistId { get; set; }
        public int? ArtistPageId { get; set; }
        public int AssociatedProductId { get; set; }
        public bool Published { get; set; }

        public virtual IList<SongPicture> Pictures { get; set; }
        public virtual ArtistPage ArtistPage { get; set; }
    }
}
