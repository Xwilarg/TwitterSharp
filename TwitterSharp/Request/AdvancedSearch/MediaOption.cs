using System;
namespace TwitterSharp.Request.AdvancedSearch
{
    public enum MediaOption
    {
        /// <summary>
        /// Contains the duration of the media if it's a video
        /// </summary>
        Duration_Ms,

        /// <summary>
        /// Height of the content in pixel
        /// </summary>
        Height,

        /// <summary>
        /// Width of the content in pixel
        /// </summary>
        Width,

        /// <summary>
        /// URL to a preview image of the content
        /// </summary>
        Preview_Image_Url
    }
}
