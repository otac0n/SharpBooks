﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpCash.Debug
{
    public partial class ChartView : Form
    {
        public ChartView()
        {
            InitializeComponent();
        }

        public void RunApplication(Dictionary<int, decimal> data)
        {
            this.UseData(data);

            Application.Run(this);
        }

        private void UseData(Dictionary<int, decimal> data)
        {
            //
        }
    }
}
