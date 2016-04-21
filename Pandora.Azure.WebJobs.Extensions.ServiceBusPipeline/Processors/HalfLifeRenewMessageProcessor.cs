using Pandora.Azure.WebJobs.PipelineCore;
using Pandora.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pandora.Azure.WebJobs.Extensions.ServiceBusPipeline.Processors
{
    public class HalfLifeRenewMessageProcessor : IMessageProcessor
    {
        public async Task Invoke(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            var done = context.Message.AutoRenew();
            try
            {
                await next();
            }
            finally
            {
                done();
            }
        }
    }
}
