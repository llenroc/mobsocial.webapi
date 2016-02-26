﻿using Nop.Core;
using Nop.Core.Domain.Seo;
using System;
using Mob.Core.Domain;

namespace Nop.Plugin.WebApi.MobSocial.Domain
{
    public class EventPageHotel : BaseMobEntity
    {

        public int EventPageId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipPostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        /// <summary>
        /// For providing additional information about the hotel. For example,
        /// 'Mention group code "SummerJam" to get the special event rate.'
        /// </summary>
        public string AdditionalInformation { get; set; }
        public int DisplayOrder { get; set; }



        public virtual EventPage EventPage { get; set; }

    }
}