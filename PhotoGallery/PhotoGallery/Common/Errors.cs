namespace PhotoGallery.Common
{
    public class Errors
    {
        public const string UserNotFound = "There is no user with username '{0}'";

        public const string PhotoNotFound = "There is no photo '{0}' owned by user '{1}'";

        public const string OwnerTriesToRate = "You can't rate your own photos";

        public const string AttemptToRemovePhoto = "You can't remove photos that do not belong to you";

        public const string AttemptToEditPhoto = "You can't edit photos that do not belong to you";

        public const string AttemptToUpdatePhoto = "You can't update photos that do not belong to you";

        public const string AttemptToAdAlbum = "You can't add photos that do not belong to you to albums";

        public const string AttemptToEditAlbum = "You can't edit albums that do not belong to you";

        public const string AttemptToUpdateAlbum = "You can't update albums that do not belong to you";

        public const string AttemptToRemoveAlbum = "You can't remove albums that do not belong to you";

        public const string Unauthorized = "You are not authorized to do this";

        public const string PhotoLimitReached = "You've reached photo limit for common user. Please consider to get a subscription";

        public const string AlbumLimitReached = "You've reached album limit for common user. Please consider to get a subscription";
    }
}
