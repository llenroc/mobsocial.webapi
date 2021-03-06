﻿using System.Collections.Generic;
using Mob.Core.Services;
using Nop.Plugin.WebApi.MobSocial.Domain;

namespace Nop.Plugin.WebApi.MobSocial.Services
{
    public interface ITeamPageGroupMemberService : IBaseEntityService<GroupPageMember>
    {
        IList<GroupPageMember> GetGroupPageMembersForTeam(int teamId);

        IList<GroupPageMember> GetGroupPageMembers(int groupId);

        void DeleteGroupPageMember(int groupId, int memberId);
    }
}