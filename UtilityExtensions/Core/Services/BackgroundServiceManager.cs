using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace UtilityExtensions.Core.Services
{
    public class BackgroundServiceManager : ServiceManager<BackgroundService>
    {
        private class ServiceHandler
        {
            public BackgroundService service;
            public BackgroundWorker worker;
        }

        private readonly List<ServiceHandler> handlers = new();

        /// <summary>
        /// </summary>
        /// <exception cref="ArgumentNullException"> </exception>
        /// <param name="service"> </param>
        public override void AddService(BackgroundService service, ServiceSettings settings = default)
        {
            base.AddService(service);

            BackgroundWorker bw = new BackgroundWorker();

            bw.DoWork += new(
            delegate (object o, DoWorkEventArgs args)
            {
                while (service.enabled && !bw.CancellationPending)
                {
                    try
                    {
                        settings.OnBeforeExecute?.Invoke(service);

                        service.Execute();

                        settings.OnAfterExecute?.Invoke(service);
                        if (bw.CancellationPending)
                        {
                            args.Cancel = true;
                            break;
                        }

                        Thread.Sleep(service.interval);
                    }
                    catch (Exception ex)
                    {
                        settings.OnError?.Invoke(service, ex);
                    }
                }
            });

            bw.RunWorkerCompleted += new(
            delegate (object o, RunWorkerCompletedEventArgs args)
            {
                if (service.enabled && !bw.CancellationPending)
                {
                    bw.RunWorkerAsync();
                }
            });

            handlers.Add(new ServiceHandler { service = service, worker = bw });
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ArgumentNullException"> </exception>
        /// <param name="services"> </param>
        public void AddServices(List<BackgroundService> services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            foreach (BackgroundService service in services)
            {
                AddService(service);
            }
        }

        public override void Run()
        {
            foreach (ServiceHandler handler in handlers)
            {
                if (!handler.worker.IsBusy)
                {
                    handler.worker.RunWorkerAsync();
                }
            }
        }
    }
}