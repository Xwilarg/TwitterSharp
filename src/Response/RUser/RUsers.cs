using System;
using System.Threading.Tasks;

namespace TwitterSharp.Response.RUser
{
    public class RUsers
    {
        public User[] Users { init; get; }
        public Func<Task<RUsers>> NextAsync { init; get; }
    }
}
