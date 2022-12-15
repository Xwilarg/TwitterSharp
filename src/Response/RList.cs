using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterSharp.Response.RUser;

namespace TwitterSharp.Response
{
    public class RList<T>
    {
        public T[] Data { get; set; }
        public Func<Task<RList<T>>> NextAsync { init; get; }

    }
}
