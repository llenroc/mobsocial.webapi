﻿namespace Nop.Plugin.WebApi.MobSocial.Services
{
    public interface IOAuthService
    {
        string Uri { get; set; }
        string SecureUri { get; set; }
        string ConsumerKey { get; set; }
        string ConsumerSecret { get; set; }
    }
}
