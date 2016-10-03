﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;
using Mob.Core;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.WebApi.MobSocial.Attributes;
using Nop.Plugin.WebApi.MobSocial.Domain;
using Nop.Plugin.WebApi.MobSocial.Enums;
using Nop.Plugin.WebApi.MobSocial.Extensions;
using Nop.Plugin.WebApi.MobSocial.Models;
using Nop.Plugin.WebApi.MobSocial.Services;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Media;
using Nop.Services.Security;

namespace Nop.Plugin.WebApi.MobSocial.Controllers
{
    [RoutePrefix("api/friends")]
    public class FriendsApiController : BaseMobApiController
    {
        private readonly IFriendService _friendService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly IPictureService _pictureService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerFollowService _customerFollowService;
        private readonly IMobSocialService _mobSocialService;

        public FriendsApiController(IFriendService friendService, IPermissionService permissionService, IWorkContext workContext, IPictureService pictureService, ICustomerService customerService, IMobSocialService mobSocialService, ICustomerFollowService customerFollowService)
        {
            _friendService = friendService;
            _permissionService = permissionService;
            _workContext = workContext;
            _pictureService = pictureService;
            _customerService = customerService;
            _mobSocialService = mobSocialService;
            _customerFollowService = customerFollowService;
        }

     

        [ApiAuthorize]
        [HttpPost]
        [Route("addfriend/{friendId:int}")]
        public IHttpActionResult AddFriend(int friendId)
        {

            if (_workContext.CurrentCustomer.Id == friendId)
                return Json(new { Success = false, Message = "Can't add one's self" });

            var fromCustomerId = _workContext.CurrentCustomer.Id;

            //first check if the request has already been sent?
            var customerFriend = _friendService.GetCustomerFriendship(fromCustomerId, friendId) ??
                                 new CustomerFriend() {
                                     FromCustomerId = fromCustomerId,
                                     ToCustomerId = friendId,
                                     DateCreated = DateTime.UtcNow,
                                     DateRequested = DateTime.UtcNow
                                 };

            if (customerFriend.Confirmed)
            {
                return Json(new { Success = false, Message = "Already friends" });
            }

            if (customerFriend.Id == 0)
            {
                _friendService.Insert(customerFriend);

                //let's add customer follow
                _customerFollowService.Insert<CustomerProfile>(fromCustomerId, friendId);
            }


            return Json(new { Success = true, NewStatus = FriendStatus.FriendRequestSent });

        }

        [ApiAuthorize]
        [HttpPost]
        [Route("confirmfriend/{friendId:int}")]
        public IHttpActionResult ConfirmFriend(int friendId)
        {

            if (_workContext.CurrentCustomer.Id == friendId)
                return Json(new { Success = false, Message = "Can't add one's self" });

            var ToCustomerId = _workContext.CurrentCustomer.Id;

            //first check if the request has already been sent?. Only the receiver can accept or decline
            var customerFriend = _friendService.GetCustomerFriend(friendId, ToCustomerId);

            if (customerFriend == null)
                return Json(new { Success = false, Message = "No friendship request sent" });

            customerFriend.Confirmed = true;
            customerFriend.DateConfirmed = DateTime.UtcNow;
            _friendService.Update(customerFriend);

            return Json(new { Success = true, NewStatus = FriendStatus.Friends });
        }

        [ApiAuthorize]
        [HttpPost]
        [Route("declinefriend/{friendId:int}")]
        public IHttpActionResult DeclineFriend(int friendId)
        {

            if (_workContext.CurrentCustomer.Id == friendId)
                return Json(new { Success = false, Message = "Can't add one's self" });

            var Customer2Id = _workContext.CurrentCustomer.Id;

            //first check if the request has already been sent?. Any of two parties can decline
            var customerFriend = _friendService.GetCustomerFriendship(friendId, Customer2Id);

            if (customerFriend == null)
                return Json(new { Success = false, Message = "No friendship request sent" });

            _friendService.Delete(customerFriend);

            return Json(new { Success = true, NewStatus = FriendStatus.None });
        }

      
        [HttpGet]
        [ApiAuthorize]
        [Route("getfriendrequests")]
        public IHttpActionResult GetFriendRequests()
        {
            var friendRequests = _friendService.GetCustomerFriendRequests(_workContext.CurrentCustomer.Id);

            var friendRequestCustomers =
                _customerService.GetCustomersByIds(friendRequests.Select(x => x.FromCustomerId).ToArray());

            var model = new List<FriendPublicModel>();
            foreach (var c in friendRequestCustomers)
            {
                var friendModel = new FriendPublicModel() {
                    Id = c.Id,
                    DisplayName = c.GetFullName(),
                    PictureUrl = _pictureService.GetPictureUrl(
                        c.GetAttribute<int>(SystemCustomerAttributeNames.AvatarPictureId)),
                    ProfileUrl =
                        Url.RouteUrl("CustomerProfileUrl",
                            new RouteValueDictionary() { {"SeName", c.GetSeName(_workContext.WorkingLanguage.Id, true, false) }}),
                    FriendStatus = FriendStatus.NeedsConfirmed
                };
                model.Add(friendModel);

            }

            return Json(new { Success = true, People = model });
        }

        [ApiAuthorize]
        [HttpGet]
        [Route("getcustomerfriends")]
        public IHttpActionResult GetCustomerFriends(int customerId , int howMany = 0, bool random = false)
        {

            if (customerId == 0)
                customerId = _workContext.CurrentCustomer.Id;

            var friends = _friendService.GetCustomerFriends(customerId);

            var model = new List<CustomerFriendModel>();

            foreach (var friend in friends)
            {

                var friendId = (friend.FromCustomerId == customerId) ? friend.ToCustomerId : friend.FromCustomerId;

                var friendCustomer = _customerService.GetCustomerById(friendId);

                if (friendCustomer == null)
                    continue;

                var friendThumbnailUrl = _pictureService.GetPictureUrl(
                        friendCustomer.GetAttribute<int>(SystemCustomerAttributeNames.AvatarPictureId),
                        100,
                        true);

                model.Add(new CustomerFriendModel() {
                    CustomerDisplayName = friendCustomer.GetFullName().ToTitleCase(),
                    ProfileUrl =  Url.RouteUrl("CustomerProfileUrl",
                            new RouteValueDictionary()
                            {
                               { "SeName",  friendCustomer.GetSeName(_workContext.WorkingLanguage.Id, true, false) }  
                            }),
                    ProfileThumbnailUrl = friendThumbnailUrl
                });

            }

            return Json(new { Success = true, Friends = model });

        }
      

        [HttpGet]
        [Route("searchpeople")]
        public IHttpActionResult SearchPeople([FromUri] FriendSearchModel model)
        {

            var customers = _mobSocialService.SearchPeople(model.SearchTerm, model.ExcludeLoggedInUser, model.Page, model.Count);
            var models = new List<object>();

            //get all the friends of logged in customer
            IList<CustomerFriend> friends = null;
            if (_workContext.CurrentCustomer.IsRegistered())
            {
                friends = _friendService.GetAllCustomerFriends(_workContext.CurrentCustomer.Id);
            }

            if (friends == null)
                friends = new List<CustomerFriend>();

            foreach (var c in customers)
            {
                var friendModel = new FriendPublicModel() {
                    Id = c.Id,
                    DisplayName = c.GetFullName(),
                    PictureUrl = _pictureService.GetPictureUrl(
                        c.GetAttribute<int>(SystemCustomerAttributeNames.AvatarPictureId)),
                    ProfileUrl =
                        Url.RouteUrl("CustomerProfileUrl",
                            new RouteValueDictionary()
                            {
                               { "SeName",  c.GetSeName(_workContext.WorkingLanguage.Id, true, false) }  
                            }),
                };

                var friend = friends.FirstOrDefault(x => x.FromCustomerId == c.Id || x.ToCustomerId == c.Id);

                if (friend == null)
                    friendModel.FriendStatus = FriendStatus.None;
                else if (friend.Confirmed)
                    friendModel.FriendStatus = FriendStatus.Friends;
                else if (!friend.Confirmed && friend.FromCustomerId == _workContext.CurrentCustomer.Id)
                    friendModel.FriendStatus = FriendStatus.FriendRequestSent;
                else if (_workContext.CurrentCustomer.Id == c.Id)
                    friendModel.FriendStatus = FriendStatus.Self;
                else
                    friendModel.FriendStatus = FriendStatus.NeedsConfirmed;
                models.Add(friendModel);
            }
            return Json(new { Success = true, People = models });
        }

    }
}