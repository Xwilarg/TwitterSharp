using System;

namespace TwitterSharp.ApiEndpoint
{
    [Flags]
    public enum AccessLevel
    {
        None = 0,
        Essential = 1,
        Elevated = 2,
        ElevatedPlus = 4,
        AcademicResearch = 8,
        All = Essential | Elevated | ElevatedPlus | AcademicResearch
    }
}
