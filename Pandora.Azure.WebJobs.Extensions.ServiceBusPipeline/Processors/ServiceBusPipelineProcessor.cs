using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Pandora.Azure.WebJobs.PipelineCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pandora.Azure.WebJobs.Extensions.ServiceBusPipeline.Processors
{
    internal class ServiceBusPipelineProcessor : MessageProcessor
    {
        #region fields
        private readonly PipelineProcessor _processor;
        #endregion

        #region constructors
        public ServiceBusPipelineProcessor(OnMessageOptions messageOptions, List<object> stages)
            : base(messageOptions)
        {
            if (stages == null)
                throw new ArgumentNullException(nameof(stages));

            _processor = new PipelineProcessor(messageOptions);

            foreach (var stage in stages)
            {
                if (stage is Type)
                    _processor.Add(stage as Type);
                else
                    _processor.Add(stage as Func<IPipelineContext, Func<Task>, CancellationToken, Task>);
            }
        }
        #endregion

        public override Task<bool> BeginProcessingMessageAsync(BrokeredMessage message, CancellationToken cancellationToken)
        {
            return _processor.BeginProcessingMessageAsync(message, cancellationToken);
        }
        public override Task CompleteProcessingMessageAsync(BrokeredMessage message, FunctionResult result, CancellationToken cancellationToken)
        {
            return _processor.CompleteProcessingMessageAsync(message, result, cancellationToken);
        }
    }
}
