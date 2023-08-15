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
        Preview_Image_Url,

        /// <summary>
        /// URL to the image of the content
        /// </summary>
        Url,

        /// <summary>
        /// Each media object may have multiple display or playback variants, with different resolutions or formats.
        /// For videos, each variant will also contain URLs to the video in each format.
        /// </summary>
        Variants
    }
}
