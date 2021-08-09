// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using SharpBooks.Plugins;
    using SharpBooks.Plugins.Persistence;

    public partial class PersistencePluginSelector : Form
    {
        private IList<IPluginFactory<IPersistenceStrategy>> plugins;

        public PersistencePluginSelector(IList<IPluginFactory<IPersistenceStrategy>> plugins)
        {
            this.plugins = plugins;

            this.InitializeComponent();

            foreach (var plugin in plugins)
            {
                this.comboBox1.Items.Add(new FactoryDisplay(plugin));
            }

            this.comboBox1.SelectedIndex = 0;
        }

        public IPluginFactory<IPersistenceStrategy> StrategyFactory { get; private set; } = null;

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.StrategyFactory = (this.comboBox1.SelectedItem as FactoryDisplay).Factory;

            this.DialogResult = DialogResult.OK;
        }

        private class FactoryDisplay
        {
            public FactoryDisplay(IPluginFactory<IPersistenceStrategy> factory)
            {
                this.Factory = factory ?? throw new ArgumentNullException(nameof(factory));
            }

            public IPluginFactory<IPersistenceStrategy> Factory { get; }

            public override string ToString()
            {
                return this.Factory.Name;
            }
        }
    }
}
