using Autofac;
using Autofac.Integration.Mef;
using Prism.Events;
using PrismAndMefExample.Service;
using PrismAndMefExample.ViewModel;
using Shared.Contracts.Services;
using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PrismAndMefExample.Startup
{
    public class Bootstrapper
    {
        public IContainer Cointainer;
        private readonly ContainerBuilder _builder;

        public Bootstrapper()
        {
            if (_builder == null) _builder = new ContainerBuilder();
        }

        public static Bootstrapper Instance { get; } = new Bootstrapper();

        public IContainer BootStrap()
        {
            RegisterTypes();
            AddAssemblies(_builder);
            Cointainer = _builder.Build();
            return Cointainer;
        }

        private void RegisterTypes()
        {
            _builder.RegisterType<MainViewModel>().AsSelf();

            _builder.RegisterType<WindowFactory>().As<IWindowFactory>().Exported(x => x.As<IWindowFactory>());
            _builder.RegisterType<DialogService>().As<IDialogService>().Exported(x => x.As<IDialogService>());
            _builder.RegisterType<ConfigurationService>().As<IConfigurationService>().Exported(x => x.As<IConfigurationService>());
            _builder.RegisterType<ScannerService>().As<IScannerService>().Exported(x => x.As<IScannerService>()).SingleInstance();
            _builder.RegisterType<EventAggregator>().As<IEventAggregator>().Exported(x => x.As<IEventAggregator>()).SingleInstance();
        }

        private void AddAssemblies(ContainerBuilder builder)
        {
            var directory = @".\Application_1";
            //var directory = @".\Application_2";

            var files = Directory.EnumerateFiles(directory, "*.dll", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                try
                {
                    var asmCat = new AssemblyCatalog(file);
                    if (asmCat.Parts.ToList().Count > 0) builder.RegisterComposablePartCatalog(asmCat);
                }
                catch (ReflectionTypeLoadException) { }
                catch (BadImageFormatException) { }
            }
        }


    }


}
