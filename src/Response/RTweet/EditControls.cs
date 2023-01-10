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
        public bool IsEditEligible { get; init; }
        public DateTime EditableUntil { get; init; }
        public int EditsRemaining { get; init; }
    }
}
