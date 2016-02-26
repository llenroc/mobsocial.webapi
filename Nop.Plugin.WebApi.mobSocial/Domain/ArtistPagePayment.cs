﻿using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mob.Core.Domain;

namespace Nop.Plugin.WebApi.MobSocial.Domain
{
    public class ArtistPagePayment : BaseMobEntity
    {
        public enum PagePaymentType
        {
            Paypal = 1,
            BankAccount = 2,
            SendCheck = 3
        }
        public int ArtistPageId { get; set; }
        public PagePaymentType PaymentType { get; set; }
        public string PaypalEmail { get; set; }
        public string BankName { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string PayableTo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public virtual ArtistPage ArtistPage { get; set; }

    }
}
