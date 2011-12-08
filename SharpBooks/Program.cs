﻿//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using SharpBooks.Controllers;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var controller = new MainController();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            controller.Run();
        }
    }
}
