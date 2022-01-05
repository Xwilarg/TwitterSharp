using System;
using System.Threading.Tasks;

namespace TwitterSharp.Response.RUser
{
    public class Follow
    {
        public User[] Users { init; get; }
        public Func<Task<Follow>> NextAsync { init; get; }
    }
}
