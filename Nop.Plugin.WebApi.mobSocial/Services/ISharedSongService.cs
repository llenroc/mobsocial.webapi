﻿using System.Collections.Generic;
using Mob.Core.Services;
using Nop.Plugin.WebApi.MobSocial.Domain;

namespace Nop.Plugin.WebApi.MobSocial.Services
{
    public interface ISharedSongService: IBaseEntityService<SharedSong>
    {
        
        IList<SharedSong> GetSharedSongs(int CustomerId, int Count = 15, int Page = 1);

        IList<SharedSong> GetSharedSongs(int CustomerId, out int TotalPages, int Count = 15, int Page = 1);

        IList<SharedSong> GetReceivedSongs(int CustomerId, int Count = 15, int Page = 1);

        IList<SharedSong> GetReceivedSongs(int CustomerId, out int TotalPages , int Count = 15, int Page = 1);
    }
}
