﻿using System;
using System.Reflection;
using Microsoft.Owin.Hosting;
using log4net;

namespace Rebus.BusHub.Hub
{
    public class BusHubService
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        readonly string url;

        public static IMessageHandler[] MessageHandlers { get; private set; }

        IDisposable webApp;

        public BusHubService(string url, IMessageHandler[] messageHandlers)
        {
            this.url = url;
            
            MessageHandlers = messageHandlers;
        }

        public void Start()
        {
            try
            {
                Log.InfoFormat("Starting up on {0}...", url);
                
                webApp = WebApplication.Start<Startup>(url);
                
                Log.InfoFormat("Server listening!");
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format("An error occurred while attempting to open SignalR hub on {0}",
                                  url), e);
            }
        }

        public void Stop()
        {
            Log.Info("Shutting down server...");

            if (webApp != null)
            {
                webApp.Dispose();
            }

            Log.Info("Server stopped!");
        }
    }
}