using System;
using System.Threading.Tasks;

namespace TwitterSharp.Response
{
    public class RArray<T>
    {
        public T[] Data { get; set; }
        public Func<Task<RArray<T>>> NextAsync { init; get; }

    }
}
