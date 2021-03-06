﻿using System;
using System.Threading.Tasks;
using questioneer.Core.Entities;

namespace questioneer
{
    internal class Program
    {
        private static Startup _startup;

        internal static async Task Main(string[] args)
        {
            Reflections.AppDomain.ProcessExit += ProcessOnExited;

            _startup = new Startup();
            await _startup.RunAsync().ConfigureAwait(false);
        }

        private static void ProcessOnExited(object sender, EventArgs eventArgs)
        {
            _startup.Dispose();
        }
    }
}