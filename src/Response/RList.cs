using System;
using System.Threading.Tasks;

namespace TwitterSharp.Response
{
    public class RList<T>
    {
        public T[] Data { get; set; }
        public Func<Task<RList<T>>> NextAsync { init; get; }

    }
}
