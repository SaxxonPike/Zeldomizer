using System;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Zeldomizer.UI.Core;
using Zeldomizer.UI.Forms;

namespace Zeldomizer.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var container = CreateIocContainer();
            var formFactory = container.Resolve<FormFactory>();
            Application.Run(formFactory.GetForm<MasterForm>());
        }

        private static IContainer CreateIocContainer()
        {
            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();
            IContainer result = null;

            // ReSharper disable once AccessToModifiedClosure
            builder.Register(c => result)
                .As<IContainer>()
                .SingleInstance();

            builder.RegisterType<Context>()
                .As<IContext>()
                .SingleInstance();

            builder.RegisterType<FileDialogFactory>()
                .As<IFileDialogFactory>()
                .SingleInstance();

            builder.RegisterType<FormFactory>()
                .As<IFormFactory>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<Form>();

            result = builder.Build();
            return result;
        }
    }
}
