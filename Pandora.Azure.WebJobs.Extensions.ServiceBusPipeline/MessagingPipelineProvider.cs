// Copyright (c) PandoraJewelry. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Azure.WebJobs.ServiceBus;
using Pandora.Azure.WebJobs.Extensions.ServiceBusPipeline.Processors;
using Pandora.Azure.WebJobs.PipelineCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pandora.Azure.WebJobs.Extensions.ServiceBusPipeline
{
    public class MessagingPipelineProvider : MessagingProvider
    {
        #region fields
        private readonly ServiceBusConfiguration _config;
        private readonly List<object> _stages;
        #endregion

        #region constructors
        public MessagingPipelineProvider(ServiceBusConfiguration config) : base(config)
        {
            _config = config;
            _stages = new List<object>();
        }
        #endregion

        #region add
        public void Add<T>() where T : IMessageProcessor
        {
            _stages.Add(typeof(T));
        }
        public void Add(Func<IPipelineContext, Func<Task>, CancellationToken, Task> stage)
        {
            if (stage == null)
                throw new ArgumentNullException(nameof(stage));

            _stages.Add(stage);
        } 
        #endregion

        public override MessageProcessor CreateMessageProcessor(string entityPath)
        {
            return new ServiceBusPipelineProcessor(_config.MessageOptions, _stages);
        }
    }
}
