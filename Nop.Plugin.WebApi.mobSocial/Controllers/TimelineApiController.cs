﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Plugin.WebApi.MobSocial.Domain;
using Nop.Plugin.WebApi.MobSocial.Extensions;
using Nop.Plugin.WebApi.MobSocial.MediaFormatters.Infrastructure;
using Nop.Plugin.WebApi.MobSocial.Models;
using Nop.Plugin.WebApi.MobSocial.Services;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Media;
using System.IO;
using System.Web;
using System.Web.Hosting;
using Mob.Core;
using NReco.VideoConverter;

namespace Nop.Plugin.WebApi.MobSocial.Controllers
{
    [RoutePrefix("api/timeline")]
    public class TimelineApiController : BaseMobApiController
    {
        private readonly ITimelineService _timelineService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly ICustomerFollowService _customerFollowService;
        private readonly IPictureService _pictureService;
        private readonly IDateTimeHelper _dateTimeHelper;

        private readonly MediaSettings _mediaSettings;

        public TimelineApiController(ITimelineService timelineService, 
            IWorkContext workContext, 
            ICustomerFollowService customerFollowService, 
            ICustomerService customerService, 
            IPictureService pictureService, 
            MediaSettings mediaSettings,
            IDateTimeHelper dateTimeHelper)
        {
            _timelineService = timelineService;
            _workContext = workContext;
            _customerFollowService = customerFollowService;
            _customerService = customerService;
            _pictureService = pictureService;
            _mediaSettings = mediaSettings;
            _dateTimeHelper = dateTimeHelper;
        }

        [Route("post")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult Post(TimelinePostModel model)
        {
            if (!ModelState.IsValid)
                return Response(new {Success = false, Message = "Invalid data"});

            //TODO: check OwnerId for valid values and store entity name accordingly, these can be customer, artist page, videobattle page etc.
            model.OwnerId = _workContext.CurrentCustomer.Id;
            model.OwnerEntityType = TimelinePostOwnerTypeNames.Customer;
           
            //create new timeline post
            var post = new TimelinePost()
            {
                Message = model.Message,
                AdditionalAttributeValue = model.AdditionalAttributeValue,
                PostTypeName = model.PostTypeName,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                OwnerId = model.OwnerId,
                IsSponsored = model.IsSponsored,
                OwnerEntityType = model.OwnerEntityType,
                LinkedToEntityName = model.LinkedToEntityName,
                LinkedToEntityId = model.LinkedToEntityId
            };

            

            //save it
            _timelineService.Insert(post);

            var postModel = new TimelinePostDisplayModel() {
                Id = post.Id,
                DateCreatedUtc = post.DateCreated,
                DateUpdatedUtc = post.DateUpdated,
                DateCreated = _dateTimeHelper.ConvertToUserTime(post.DateCreated, DateTimeKind.Utc),
                DateUpdated = _dateTimeHelper.ConvertToUserTime(post.DateUpdated, DateTimeKind.Utc),
                OwnerId = post.OwnerId,
                OwnerEntityType = post.OwnerEntityType,
                PostTypeName = post.PostTypeName,
                IsSponsored = post.IsSponsored,
                Message = post.Message,
                AdditionalAttributeValue = post.AdditionalAttributeValue
            };
            if (post.OwnerEntityType == TimelinePostOwnerTypeNames.Customer)
            {
                //get the customer to retrieve info such a profile image, profile url etc.
                var customer = _customerService.GetCustomerById(post.OwnerId);

                postModel.OwnerName = customer.GetFullName();
                postModel.OwnerImageUrl =
                    _pictureService.GetPictureUrl(
                        customer.GetAttribute<int>(SystemCustomerAttributeNames.AvatarPictureId),
                        _mediaSettings.AvatarPictureSize, false);

                postModel.OwnerProfileUrl = Url.RouteUrl("CustomerProfileUrl", new RouteValueDictionary()
                    {
                        {"SeName", customer.GetSeName(_workContext.WorkingLanguage.Id, true, false)}
                    });
            }

            return Response(new {Success = true, Post = postModel});
        }

        [Route("get")]
        [HttpGet]
        public IHttpActionResult Get([FromUri] TimelinePostsRequestModel model)
        {
            var customerId = model.CustomerId;
            var page = model.Page;
            //we need to get posts from followers, friends, self, and anything else that supports posting to timeline
            
            //but we need to know if the current customer is a registered user or a guest
            var isRegistered = _workContext.CurrentCustomer.IsRegistered();
            //the posts that'll be returned
            List<TimelinePost> timelinePosts = null;

            //the number of posts
            var count = 15;
            //if the user is registered, then depending on value of customerId, we fetch posts. 
            //{if the customer id matches the logged in user id, he is seeing his own profile, so we show the posts by her only
            //{if the customer id is zero, then we show posts by her + posts by her friends + posts by people she is following etc.
            //{if the customer id is non-zero, then we show posts by the customer of customerId
            if (isRegistered)
            {
                if (_workContext.CurrentCustomer.Id == customerId)
                {
                    timelinePosts = _timelineService.GetAll(string.Empty, count, page);
                }
                else
                {
                    if (customerId != 0)
                    {
                        //we need to get posts by this customer
                        timelinePosts = _timelineService.GetByEntityIds("customer", new[] {customerId}).ToList();
                    }
                    else
                    {
                        //we need to find he person she is following.
                        var allFollows = _customerFollowService.GetCustomerFollows<CustomerProfile>(customerId);

                        //get all the customer's ids which she is following
                        var customerIds =
                            allFollows.Where(x => x.FollowingEntityName == typeof (CustomerProfile).Name)
                                .Select(x => x.FollowingEntityId).ToList();

                        //and add current customer has well to cover her own posts
                        customerIds.Add(_workContext.CurrentCustomer.Id);
                        

                        //get timeline posts
                        timelinePosts = _timelineService.GetByEntityIds("customer", customerIds.ToArray(), count, page).ToList();

                    }
                }
            }
            else
            {
                //should we show the data to non logged in user?
                return null;
            }

            var responseModel = new List<TimelinePostDisplayModel>();

            foreach (var post in timelinePosts)
            {
                var postModel = new TimelinePostDisplayModel()
                {
                    Id = post.Id,
                    DateCreatedUtc = post.DateCreated,
                    DateUpdatedUtc = post.DateUpdated,
                    DateCreated = _dateTimeHelper.ConvertToUserTime(post.DateCreated, DateTimeKind.Utc),
                    DateUpdated = _dateTimeHelper.ConvertToUserTime(post.DateUpdated, DateTimeKind.Utc),
                    OwnerId = post.OwnerId,
                    OwnerEntityType = post.OwnerEntityType,
                    PostTypeName = post.PostTypeName,
                    IsSponsored = post.IsSponsored,
                    Message = post.Message,
                    AdditionalAttributeValue = post.AdditionalAttributeValue
                };
                if (post.OwnerEntityType == TimelinePostOwnerTypeNames.Customer)
                {
                    //get the customer to retrieve info such a profile image, profile url etc.
                    var customer = _customerService.GetCustomerById(post.OwnerId);

                    postModel.OwnerName = customer.GetFullName();
                    postModel.OwnerImageUrl =
                        _pictureService.GetPictureUrl(
                            customer.GetAttribute<int>(SystemCustomerAttributeNames.AvatarPictureId),
                            _mediaSettings.AvatarPictureSize, false);

                    postModel.OwnerProfileUrl = Url.RouteUrl("CustomerProfileUrl", new RouteValueDictionary()
                    {
                        {"SeName", customer.GetSeName(_workContext.WorkingLanguage.Id, true, false)}
                    });
                }
                responseModel.Add(postModel);
            }

            return Response(responseModel);
        }

        [Authorize]
        [Route("delete/{timelinePostId:int}")]
        [HttpDelete]
        public IHttpActionResult Delete(int timelinePostId)
        {
            //first get the timeline post
            var post = _timelineService.GetById(timelinePostId);

            if (post == null)
                return Response(new {Success = false, Message = "Post doesn't exist"});

            //only admin or post owner should be able to delete the post
            if (post.OwnerId == _workContext.CurrentCustomer.Id || _workContext.CurrentCustomer.IsAdmin())
            {
                _timelineService.Delete(post);

                return Response(new {Success = true});
            }
            return Response(new {Success = false, Message = "Unauthorized"});
        }

        [Authorize]
        [Route("uploadpictures")]
        [HttpPost]
        public IHttpActionResult UploadPictures(IList<HttpFile> files)
        {
            var newImages = new List<object>();
            for (var index = 0; index < files.Count; index++)
            {

                //the file
                var file = files[index];

                //and it's name
                var fileName = file.FileName;

                //stream to read the bytes
                var pictureBytes = file.Buffer;

                //file extension and it's type
                var fileExtension = Path.GetExtension(fileName);
                if (!string.IsNullOrEmpty(fileExtension))
                    fileExtension = fileExtension.ToLowerInvariant();

                var contentType = file.MediaType;

                if (string.IsNullOrEmpty(contentType))
                {
                    contentType = PictureUtility.GetContentType(fileExtension);
                }
                //save the picture now
                var picture = _pictureService.InsertPicture(pictureBytes, contentType, null);
                
                newImages.Add(new {
                    ImageUrl = _pictureService.GetPictureUrl(picture.Id),
                    ImageId = picture.Id
                });
            }

            return Json(new { Success = true, Images = newImages });
        }

        [Authorize]
        [Route("uploadvideo")]
        [HttpPost]
        public IHttpActionResult UploadVideo()
        {
            var files = HttpContext.Current.Request.Files;
            if (files.Count == 0)
                return Response(new {Success = false, Message = "No file uploaded" });

            var file = files[0];
            //and it's name
            var fileName = file.FileName;


            //file extension and it's type
            var fileExtension = Path.GetExtension(fileName);
            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            var contentType = file.ContentType;

            if (string.IsNullOrEmpty(contentType))
            {
                contentType = VideoUtility.GetContentType(fileExtension);
            }

            if (contentType == string.Empty)
            {
                return Response(new { Success = false, Message = "Invalid file type" });
            }

            var tickString = DateTime.Now.Ticks.ToString();
            var savePath = ControllerUtil.MobSocialPluginsFolder + "Uploads/" + tickString + fileExtension;
            file.SaveAs(HostingEnvironment.MapPath(savePath));

            //wanna generate the thumbnails for videos...ffmpeg is our friend
            var ffmpeg = new FFMpegConverter();
            var thumbnailFilePath = ControllerUtil.MobSocialPluginsFolder + "Uploads/" + tickString + ".thumb.jpg";
            ffmpeg.GetVideoThumbnail(HostingEnvironment.MapPath(savePath), HostingEnvironment.MapPath(thumbnailFilePath));
            //save the picture now

          return Json(new { 
              Success = true, 
              VideoUrl = savePath.Replace("~", ""),
              ThumbnailUrl = thumbnailFilePath.Replace("~", ""),
              MimeType = file.ContentType
          });
        }
    }
}