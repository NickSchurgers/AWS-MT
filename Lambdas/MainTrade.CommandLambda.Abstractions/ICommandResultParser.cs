using Amazon.Lambda.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLambda
{
    public interface ICommandResultParser<T>
    {
        public Task<T> ParseAsync(InvokeResponse response);
    }
}
