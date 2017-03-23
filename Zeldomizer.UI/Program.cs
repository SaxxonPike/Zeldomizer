using System;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Zeldomizer.UI.Browser;

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
            var browserForm = container.Resolve<BrowserUi>();
            Application.Run(browserForm);
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

            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<Form>();

            result = builder.Build();
            return result;
        }
    }
}
