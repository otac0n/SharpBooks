namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using SharpBooks.Plugins;

    public partial class PersistencePluginSelector : Form
    {
        private IList<IPersistenceStrategyFactory> plugins;
        private IPersistenceStrategyFactory strategyFactory = null;

        public PersistencePluginSelector(IList<IPersistenceStrategyFactory> plugins)
        {
            this.plugins = plugins;

            InitializeComponent();

            foreach (var plugin in plugins)
            {
                this.comboBox1.Items.Add(new FactoryDisplay(plugin));
            }

            this.comboBox1.SelectedIndex = 0;
        }

        public IPersistenceStrategyFactory StrategyFactory
        {
            get { return this.strategyFactory; }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.strategyFactory = (this.comboBox1.SelectedItem as FactoryDisplay).Factory;

            this.DialogResult = DialogResult.OK;
        }

        private class FactoryDisplay
        {
            private readonly IPersistenceStrategyFactory factory;

            public FactoryDisplay(IPersistenceStrategyFactory factory)
            {
                if (factory == null)
                {
                    throw new ArgumentNullException("factory");
                }

                this.factory = factory;
            }

            public IPersistenceStrategyFactory Factory
            {
                get
                {
                    return this.factory;
                }
            }

            public override string ToString()
            {
                return this.factory.Name;
            }
        }
    }
}
