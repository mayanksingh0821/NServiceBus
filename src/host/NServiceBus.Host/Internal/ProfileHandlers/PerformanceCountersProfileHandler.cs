﻿using System;
using System.Diagnostics;
using NServiceBus.Host.Profiles;
using NServiceBus.Unicast.Transport.Msmq;

namespace NServiceBus.Host.Internal.ProfileHandlers
{
    /// <summary>
    /// Handles the PerformanceCounters profile.
    /// </summary>
    public class PerformanceCountersProfileHandler : IHandleProfile<PerformanceCounters>
    {
        /// <summary>
        /// Registers the performance counter.
        /// </summary>
        /// <param name="specifier"></param>
        public void Init(IConfigureThisEndpoint specifier)
        {
            var categoryName = "NServiceBus";
            var counterName = "Critical Time";


			var counter = new PerformanceCounter(categoryName, counterName, Program.GetEndpointId(specifier),
                                                 false);

            GenericHost.ConfigurationComplete += (o, e) =>
                                                     {
                     var msmqTransport = Configure.ObjectBuilder.Build<MsmqTransport>();
                     msmqTransport.TransportMessageReceived += (obj, args) =>
                       {
                           counter.RawValue =
                               Convert.ToInt32((DateTime.Now - args.Message.TimeSent).TotalSeconds);

                       };
                 };
    }
    }
}