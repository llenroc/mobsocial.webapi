﻿using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Plugin.WebApi.MobSocial.Enums;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.WebApi.MobSocial.Models
{
    public class CustomerPaymentPublicModel : BaseNopModel
    {
        public CustomerPaymentPublicModel()
        {
            CustomerPaymentMethods = new List<SelectListItem>();
            CustomerPendingOrders = new List<SelectListItem>();
        }

        public IList<SelectListItem> CustomerPaymentMethods { get; set; }

        public IList<SelectListItem> CustomerPendingOrders { get; set; }

        public decimal MinimumPaymentAmount { get; set; }

        public bool IsAmountVariable { get; set; }

        public PurchaseType PurchaseType { get; set; }

    }
}