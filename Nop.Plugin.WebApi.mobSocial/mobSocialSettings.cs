﻿
using Nop.Core.Configuration;

namespace Nop.Plugin.WebApi.MobSocial
{
    public class mobSocialSettings : ISettings
    {
        public int ProfilePictureSize { get; set; }
        public string WidgetZone { get; set; }

        public bool ShowProfileImagesInSearchAutoComplete { get; set; }

        public int PeopleSearchTermMinimumLength { get; set; }

        public int PeopleSearchAutoCompleteNumberOfResults { get; set; }

        public bool PeopleSearchAutoCompleteEnabled { get; set; }

        public int CustomerAlbumPictureThumbnailWidth { get; set; }

        public int MaximumMainAlbumPictures { get; set; }

        public int MaximumMainAlbumVideos { get; set; }

        public int EventPageSearchTermMinimumLength { get; set; }

        public int EventPageSearchAutoCompleteNumberOfResults { get; set; }

        public int EventPageAttendanceThumbnailSize { get; set; }

        public int UninvitedFriendsNumberOfResults { get; set; }

        public int CustomerProfileUpdateViewCountAfterNumberOfSeconds { get; set; }

        public string FacebookWebsiteAppId { get; set; }

        public string BusinessPageSearchUrl { get; set; }

        public string EchonestAPIKey { get; set; }

        public int ArtistPageMainImageSize { get; set; }

        public int ArtistPageThumbnailSize { get; set; }

        public string SevenDigitalPartnerId { get; set; }

        public int SongFileMaximumUploadSize { get; set; }

        public int SongFileSampleMaximumUploadSize { get; set; }

        public int PurchasedSongFeePercentage { get; set; }

        public bool ShowVideoThumbnailsForBattles { get; set; }

        public decimal DefaultVotingChargeForPaidVoting { get; set; }

        public bool EnableAutomaticMigrations { get; set; }

        public bool EnableFacebookInvite { get; set; }

        public decimal Version { get; set; }

        //TODO: move these setings to a separate payment processing plugin
        public decimal MicroPaymentsFixedPaymentProcessingFee { get; set; }

        public decimal MicroPaymentsPaymentProcessingPercentage{ get; set; }

        public decimal MacroPaymentsFixedPaymentProcessingFee { get; set; }

        public decimal MacroPaymentsPaymentProcessingPercentage { get; set; }

        public decimal SiteOwnerSponsorshipPercentage { get; set; }

        public decimal BattleHostSponsorshipPercentage { get; set; }

        public int VideoUploadReminderEmailThresholdDays { get; set; }

        public int BattleVoteReminderEmailThresholdDays { get; set; }

    }
}