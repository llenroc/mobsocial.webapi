﻿using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Nop.Plugin.WebApi.MobSocial.Models
{
    public class EventPageHotelModel : BaseNopEntityModel
    {

        public string Name { get; set; }

        public string Title { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipPostalCode { get; set; }

        public string PhoneNumber { get; set; }

        [AllowHtml]
        public string AdditionalInformation { get; set; }

        public string Country { get; set; }

        public int DisplayOrder { get; set; }

        public int EventPageId { get; set; }

    }
}
