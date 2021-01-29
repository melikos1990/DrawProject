using System;
using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using SMARTII.Socket;

namespace SMARTII
{
    public partial class Startup
    {
        private readonly String _Route = "/signalr";

        public void ConfigureSignalR(IAppBuilder app, ILifetimeScope container)
        {
            app.Map(_Route, map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                    EnableDetailedErrors = true,
                    EnableJavaScriptProxies = false,
                    Resolver = new AutofacDependencyResolver(container)
                };

                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });

            var hub = container.Resolve<SignalRHub>();

            GlobalHost.DependencyResolver = new AutofacDependencyResolver(container);
        }
    }
}