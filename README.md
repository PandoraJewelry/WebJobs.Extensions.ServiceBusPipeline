# Azure Webjobs SDK Pipelines for Azure ServiceBus

## Introduction
This extension will enable pipelined processing of triggered ServiceBus messages.

Once referenced you can enable it on the `MessagingPipelineProvider` object.

    var config = new JobHostConfiguration();
    var sbc = new ServiceBusConfiguration();
    var pipe = new MessagingPipelineProvider(sbc);

    sbc.MessagingProvider = pipe;
    pipe.Add<HalfLifeRenewMessageProcessor>();
    // or
    pipe.Add(async (ctx, next, token) =>
    {
        Console.WriteLine("hello trigger");
        await next();
        Console.Write("goodby trigger");
    });

    config.UseServiceBus(sbc);
    JobHost host = new JobHost(config);
    host.RunAndBlock();

Built-in pipelines

1. `HalfLifeRenewMessageProcessor`: automaticaly renew the message every halflife

## Our use case
We use [Azure Service Bus](1) to offload long running processes. Often these processes can take longer than the max run length for ServiceBus. We opted to implement a [Pipelined](2) processor instead of a just a single dedicated `HalfLifeProcessor` for future flexibility internally.

## Installation
You can obtain it [through Nuget](https://www.nuget.org/packages/Pandora.Azure.WebJobs.Extensions.ServiceBusPipeline/) with:

    Install-Package Pandora.Azure.WebJobs.Extensions.ServiceBusPipeline

Or **clone** this repo and reference it.

## Refrences
  1. https://github.com/Azure/azure-webjobs-sdk
  2. https://github.com/Azure/azure-webjobs-sdk-extensions
[1]: https://azure.microsoft.com/en-us/documentation/services/service-bus/
[2]: https://github.com/PandoraJewelry/WebJobs.PipelineCore
