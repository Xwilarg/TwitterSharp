using System;

namespace TwitterSharp.ApiEndpoint
{
    [Flags]
    public enum AccessLevel
    {
        None = 0,
        Essential = 1,
        Elevated = 2,
        AcademicResearch = 4,
        All = Essential | Elevated | AcademicResearch
    }
}
