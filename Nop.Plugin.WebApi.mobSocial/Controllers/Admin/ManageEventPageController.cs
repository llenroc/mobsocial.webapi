﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Admin.Controllers;
using Nop.Core;
using Nop.Plugin.WebApi.MobSocial.Domain;
using Nop.Plugin.WebApi.MobSocial.Models;
using Nop.Plugin.WebApi.MobSocial.Services;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.WebApi.MobSocial.Controllers.Admin
{
    public partial class ManageEventPageController : BaseAdminController
    {
        
        #region Fields
        private readonly IEventPageService _eventPageService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly IUrlRecordService _urlRecordService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IPictureService _pictureService;
        private readonly IEventPageHotelService _eventPageHotelService;
        #endregion

        #region Constructors

        public ManageEventPageController(IPermissionService permissionService,
            IEventPageService eventPageService,
            IWorkContext workContext,
            IUrlRecordService urlRecordService,
            ILocalizationService localizationService,
            ICustomerActivityService customerActivityService,
            IPictureService pictureService,
            IEventPageHotelService eventPageHotelService)
        {
            _permissionService = permissionService;
            _eventPageService = eventPageService;
            _workContext = workContext;
            _urlRecordService = urlRecordService;
            _localizationService = localizationService;
            _customerActivityService = customerActivityService;
            _pictureService = pictureService;
            _eventPageHotelService = eventPageHotelService;
        }

        #endregion

        #region Utilities

        //page not found
        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;

            return View();
        }

     

        #endregion

        #region Methods

        [HttpPost]
        public ActionResult GetAll(DataSourceRequest command)
        {

            var entities = _eventPageService.GetAll();

            var models = new List<object>();

            foreach(var entity in entities)
            {
                var entityPicture = _eventPageService.GetFirstEntityPicture(entity.Id);
                var defaultPicture = (entityPicture != null) ? _pictureService.GetPictureById(entityPicture.PictureId) : null;

                var model = new {
                    Id = entity.Id,
                    PictureThumbnailUrl = _pictureService.GetPictureUrl(defaultPicture, 75, true),
                    Name = entity.Name,
                    Address = entity.LocationName + " " + entity.LocationAddress1 + " " + entity.LocationAddress2 + " " + 
                              entity.LocationCity + ", " + entity.LocationState + " " + 
                              entity.LocationZipPostalCode + " " + entity.LocationCountry,
                    LocationContactInfo = entity.LocationPhone + " " + entity.LocationEmail + " " + entity.LocationWebsite,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    DateCreated = entity.DateCreated.ToString(),
                    DateUpdated = entity.DateUpdated.ToString(),
                };

                models.Add(model);
            }

            
            var gridModel = new DataSourceResult
            {
                Data = models,
                Total = entities.Count()
            };

            return Json(gridModel);
        }

        [AdminAuthorize]
        public ActionResult List()
        {
            return View("~/Plugins/Widgets.mobSocial/Views/mobSocial/Admin/ManageEventPage/List.cshtml");
        }

        [AdminAuthorize]
        public ActionResult Create()
        {
            // todo add permissions later
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            var model = new EventPageModel();
            return View("~/Plugins/Widgets.mobSocial/Views/mobSocial/Admin/ManageEventPage/Create.cshtml", model);
        }


        [AdminAuthorize]
        public ActionResult Edit(int id)
        {
            //todo: add later
            //if (!_permissionService.Authorize(MobSocialPermissionProvider.ManageEventPages))
            //    return AccessDeniedView();

            var item = _eventPageService.GetById(id);

            if (item == null) //Not found
                return RedirectToAction("List");


            var model = new EventPageModel()
            {
                Id = item.Id,
                Name = item.Name,
                SeName = SeoExtensions.GetSeName(item, _workContext.WorkingLanguage.Id),
                LocationName = item.LocationName,
                Address1 = item.LocationAddress1,
                Address2 = item.LocationAddress2,
                City = item.LocationCity,
                LocationState = item.LocationState,
                ZipPostalCode = item.LocationZipPostalCode,
                LocationCountry = item.LocationCountry,
                Phone = item.LocationPhone,
                Website = item.LocationWebsite,
                Email = item.LocationEmail,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                Description = item.Description,
                DateCreated = item.DateCreated,
                DateUpdated = item.DateUpdated,
                MetaDescription = item.MetaDescription,
                MetaKeywords = item.MetaKeywords,
                
            };

            return View("~/Plugins/Widgets.mobSocial/Views/mobSocial/Admin/ManageEventPage/Edit.cshtml", model);

        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(EventPageModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var entity = new EventPage()
                {
                    Name = model.Name,
                    LocationName = model.LocationName,
                    LocationAddress1 = model.LocationAddress1,
                    LocationAddress2 = model.LocationAddress2,
                    LocationCity = model.City,
                    LocationState = model.LocationState,
                    LocationZipPostalCode = model.ZipPostalCode,
                    LocationPhone = model.Phone,
                    LocationWebsite = model.Website,
                    LocationEmail = model.Email,
                    LocationCountry = model.LocationCountry,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Description = model.Description,
                    MetaKeywords = model.MetaKeywords,
                    MetaDescription = model.MetaDescription,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                };

                //search engine name
                model.SeName = SeoExtensions.GetSeName(entity, _workContext.WorkingLanguage.Id);
             
                // todo: add event hosts
                _eventPageService.Insert(entity);

                //save hotels
                //SaveEventHotels(product, ParseProductTags(model.ProductTags));
                
                //activity log
                //_customerActivityService.InsertActivity("AddNewProduct", _localizationService.GetResource("ActivityLog.AddNewProduct"), product.Name);
                _customerActivityService.InsertActivity("AddNewEventPage", _localizationService.GetResource("ActivityLog.AddNewEventPage"), entity.Name);


                SuccessNotification(_localizationService.GetResource("Admin.MobSocial.EventPage.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = entity.Id }) : RedirectToAction("List");
            }

            return View("~/Plugins/Widgets.mobSocial/Views/mobSocial/Admin/ManageEventPage/Create.cshtml", model);

        }




        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(EventPageModel model, bool continueEditing)
        {
            //todo: add later
            //if (!_permissionService.Authorize(MobSocialPermissionProvider.ManageEventPages))
            //    return AccessDeniedView();

            var item = _eventPageService.GetById(model.Id);
            if (item == null)
                //No product found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {


                item.Name = model.Name;
                item.LocationName = model.LocationName;
                item.LocationAddress1 = model.Address1;
                item.LocationAddress2 = model.Address2;
                item.LocationCity = model.City;
                item.LocationState = model.LocationState;
                item.LocationZipPostalCode = model.ZipPostalCode;
                item.LocationCountry = model.LocationCountry;
                item.LocationPhone = model.Phone;
                item.LocationWebsite = model.Website;
                item.LocationEmail = model.Email;
                item.StartDate = model.StartDate;
                item.EndDate = model.EndDate;
                item.Description = model.Description;
                item.MetaKeywords = model.MetaKeywords;
                item.MetaDescription = model.MetaDescription;
                
                item.DateUpdated = DateTime.Now;

            
                _eventPageService.Update(item);

                //search engine name
                model.SeName = SeoExtensions.GetSeName(item, _workContext.WorkingLanguage.Id);
             
                _urlRecordService.SaveSlug(item, model.SeName, _workContext.WorkingLanguage.Id);

                //picture seo names
                //UpdatePictureSeoNames(product);

                SuccessNotification(_localizationService.GetResource("Admin.MobSocial.EventPage.Updated"));

                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabName();

                    return RedirectToAction("Edit", new { id = item.Id });
                }
                else
                {
                    return RedirectToAction("List");
                }
            }

            return View("~/Plugins/Widgets.mobSocial/Views/mobSocial/Admin/ManageEventPage/Edit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            //todo: add later
            //if (!_permissionService.Authorize(MobSocialPermissionProvider.ManageEventPages))
            //    return AccessDeniedView();


            var item = _eventPageService.GetById(id);
            if (item == null)
                return RedirectToAction("List");

            _eventPageService.Delete(item);

            //activity log
            _customerActivityService.InsertActivity("DeleteEventPage", _localizationService.GetResource("ActivityLog.DeleteEventPage"), item.Name);

            SuccessNotification(_localizationService.GetResource("Admin.MobSocial.EventPage.Deleted"));
            return RedirectToAction("List");
        }
       
        #endregion


        #region Pictures
        public ActionResult PictureAdd(int pictureId, int displayOrder, int entityId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            if (pictureId == 0)
                throw new ArgumentException();

            var entity = _eventPageService.GetById(entityId);
            if (entity == null)
                throw new ArgumentException("No event page found with the specified id");

            // ...

            _eventPageService.InsertPicture(new EventPagePicture()
            {
                PictureId = pictureId,
                EntityId = entityId,
                DisplayOrder = displayOrder,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            });

            _pictureService.SetSeoFilename(pictureId, _pictureService.GetPictureSeName(entity.Name));

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PictureList(DataSourceRequest command, int entityId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            // TODO: We may want to support vendors and multiple stores in the future

            var pictures = _eventPageService.GetAllPictures(entityId);

            var picturesModel = pictures
                .Select(x =>
                {
                    return new PictureModel()
                    {
                        Id = x.Id,
                        EntityId = x.EntityId,
                        PictureId = x.PictureId,
                        PictureUrl = _pictureService.GetPictureUrl(x.PictureId),
                        DisplayOrder = x.DisplayOrder
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = picturesModel,
                Total = picturesModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult PictureUpdate(PictureModel model)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            var picture = _eventPageService.GetPictureById(model.Id);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");

            picture.DisplayOrder = model.DisplayOrder;
            picture.DateUpdated = DateTime.Now;

            _eventPageService.UpdatePicture(picture);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult PictureDelete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            var entityPicture = _eventPageService.GetPictureById(id);
            if (entityPicture == null)
                throw new ArgumentException("No picture found with the specified id");

            var pictureId = entityPicture.PictureId;

            _eventPageService.DeletePicture(entityPicture);
            var picture = _pictureService.GetPictureById(pictureId);
            _pictureService.DeletePicture(picture);

            return new NullJsonResult();
        }
        #endregion



        #region Hotels
        public ActionResult HotelAdd(EventPageHotelModel model)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            if (model == null)
                throw new ArgumentException();


            var entity = new EventPageHotel()
            {
                EventPageId = model.EventPageId,
                Title = model.Title,
                Name = model.Name,
                Address1 = model.Address1,
                Address2 = model.Address2,
                City = model.City,
                State = model.State,
                ZipPostalCode = model.ZipPostalCode,
                Country = model.Country,
                PhoneNumber = model.PhoneNumber,
                AdditionalInformation = model.AdditionalInformation,
                DisplayOrder = model.DisplayOrder,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
            };


            _eventPageHotelService.Insert(entity);

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult HotelList(DataSourceRequest command, int eventPageId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            var hotels = _eventPageService.GetById(eventPageId).Hotels;

            var model = hotels
                .Select(x =>
                {
                    return new EventPageHotelModel()
                    {
                        Id = x.Id,
                        EventPageId = x.EventPageId,
                        Title = x.Title,
                        Name = x.Name,
                        Address1 = x.Address1,
                        Address2 = x.Address2,
                        City = x.City,
                        State = x.State,
                        ZipPostalCode = x.ZipPostalCode,
                        Country = x.Country,
                        PhoneNumber = x.PhoneNumber,
                        DisplayOrder = x.DisplayOrder,
                        AdditionalInformation = x.AdditionalInformation
                    };
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = model,
                Total = model.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult HotelUpdate(EventPageHotelModel model)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            var hotel = _eventPageHotelService.GetById(model.Id);

            if (hotel == null)
                throw new ArgumentException("No hotel found with the specified id");

            hotel.Name = model.Name;
            hotel.DisplayOrder = model.DisplayOrder;
            hotel.DateUpdated = DateTime.Now;

            _eventPageHotelService.Update(hotel);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult HotelDelete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            var hotel = _eventPageHotelService.GetById(id);

            if (hotel == null)
                throw new ArgumentException("No hotel found with the specified id");

            _eventPageHotelService.Delete(hotel);

            return new NullJsonResult();
        }
        #endregion



    }
}
