namespace SharpBooks.StandardPlugins
{
    using System;
    using Microsoft.Win32;
    using SharpBooks.Persistence;

    public abstract class FilePersistenceStrategy : SimplePersistenceStrategy
    {
        public override Uri Open(Uri recentUri)
        {
            var dialog = new OpenFileDialog();
            return GetFileUrl(dialog, recentUri);
        }

        public override Uri SaveAs(Uri recentUri)
        {
            var dialog = new SaveFileDialog();
            return GetFileUrl(dialog, recentUri);
        }

        private static Uri GetFileUrl(FileDialog dialog, Uri recentUri)
        {
            dialog.Filter = "XML Files (*.xml)|*.xml";

            if (recentUri.IsFile)
            {
                dialog.FileName = recentUri.LocalPath;
            }

            var result = dialog.ShowDialog();

            if (result == true)
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
