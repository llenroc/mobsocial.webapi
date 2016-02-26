﻿using System.Collections.Generic;
using Mob.Core.Services;
using Nop.Plugin.WebApi.MobSocial.Domain;

namespace Nop.Plugin.WebApi.MobSocial.Services
{
    public interface IFriendService : IBaseEntityService<CustomerFriend>
    {
        CustomerFriend GetCustomerFriendship(int Customer1Id, int Customer2Id);

        CustomerFriend GetCustomerFriend(int FromCustomerId, int ToCustomerId);

        IList<CustomerFriend> GetCustomerFriendRequests(int CustomerId);

        IList<CustomerFriend> GetCustomerFriends(int CustomerId, int Count = 0, bool Random = false);

        IList<CustomerFriend> GetAllCustomerFriends(int CustomerId);
    }
}