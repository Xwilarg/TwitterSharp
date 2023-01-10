using System;

namespace TwitterSharp.Response.RTweet
{
    /// <summary>
    /// You can get information such as whether a Tweet was editable at the time it was created, how much time,
    /// if any, is remaining for a Tweet to be edited, and how many edits remain by specifying edit_controls in
    /// your tweet.fields parameter.
    /// </summary>
    public class EditControls
    {
        /// <summary>
        /// Tweet was editable at the time it was created
        /// </summary>
        public bool IsEditEligible { get; init; }
        /// <summary>
        /// How much time, if any, is remaining for a Tweet to be edited
        /// </summary>
        public DateTime EditableUntil { get; init; }
        /// <summary>
        /// How many edits remain
        /// </summary>
        public int EditsRemaining { get; init; }
    }
}
