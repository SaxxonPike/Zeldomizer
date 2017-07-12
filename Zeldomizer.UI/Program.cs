using System;
using System.Windows.Forms;
using Zeldomizer.Randomization;
using Zeldomizer.UI.Controllers;
using Zeldomizer.UI.Models;
using Zeldomizer.UI.Presenters;

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

            var randomizer = new Randomizer();
            var mainModel = new MainModel(randomizer);
            var mainController = new MainController(mainModel);
            var mainPresenter = new MainPresenter(mainController, mainModel);
            Application.Run(mainPresenter.Form);
        }
    }
}
