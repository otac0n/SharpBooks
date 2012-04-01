﻿//-----------------------------------------------------------------------
// <copyright file="NewBookWizard.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using SharpBooks.Controllers;

    public partial class EditAccountView : Form
    {
        public EditAccountView(MainController owner, Account account)
        {
            InitializeComponent();
        }
    }
}
