﻿using System;
using Nop.Core.Plugins;
using Nop.Services.Tasks;

namespace Nop.Plugin.WebApi.MobSocial.Tasks
{
    public class FriendRequestNotificationTask : ITask
    {

        private readonly IPluginFinder _pluginFinder;

        public FriendRequestNotificationTask(IPluginFinder pluginFinder)
        {
            this._pluginFinder = pluginFinder;
        }

        /// <summary>
        /// Execute task
        /// </summary>
        public void Execute()
        {
            //is plugin installed?
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("Widgets.mobSocial");
            if (pluginDescriptor == null)
                return;

            //plugin
            var plugin = pluginDescriptor.Instance() as MobSocialWebApiPlugin;
            if (plugin == null)
                return;

            plugin.SendFriendRequestNotifications();
        }
    }
}

