namespace SharpBooks.StandardPlugins
{
    using System;
    using System.Windows.Forms;
    using SharpBooks.Persistence;

    public abstract class FilePersistenceStrategy : SimplePersistenceStrategy
    {
        protected abstract string FileFilter
        {
            get;
        }

        public override Uri Open(Uri recentUri)
        {
            var dialog = new OpenFileDialog();
            return this.GetFileUrl(dialog, recentUri);
        }

        public override Uri SaveAs(Uri recentUri)
        {
            var dialog = new SaveFileDialog();
            return this.GetFileUrl(dialog, recentUri);
        }

        private Uri GetFileUrl(FileDialog dialog, Uri recentUri)
        {
            dialog.Filter = this.FileFilter;

            if (recentUri != null && recentUri.IsFile)
            {
                dialog.FileName = recentUri.LocalPath;
            }

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                return new Uri(dialog.FileName);
            }
            else
            {
                return null;
            }
        }
    }
}
