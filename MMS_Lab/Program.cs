using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MMS_Lab.Controller;
using MMS_Lab.View;
using MMS_Lab.Model;

namespace MMS_Lab
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

            MainWindow view = new MainWindow();
            MMS_Lab.Model.Model model = new MMS_Lab.Model.Model();
            MMS_Lab.Controller.Controller controller = new MMS_Lab.Controller.Controller(model,view);

            Application.Run(view);

        }
    }
}
