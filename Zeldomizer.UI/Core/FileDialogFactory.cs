using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Zeldomizer.UI.Core
{
    public class FileDialogFactory : IFileDialogFactory
    {
        private void SetDefaultProperties(FileDialog dialog, params (string extension, string name)[] filters)
        {
            var allFilters = string.Join("|", filters.Select(f => $"{f.name} ({f.extension})|{f.extension}"));

            dialog.AddExtension = true;
            dialog.AutoUpgradeEnabled = true;
            dialog.CheckPathExists = true;
            dialog.Filter = allFilters;
            dialog.SupportMultiDottedExtensions = true;
        }

        public OpenFileDialog GetOpen(params (string extension, string name)[] filters)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true
            };

            SetDefaultProperties(dialog, filters);
            return dialog;
        }

        public SaveFileDialog GetSave(params (string extension, string name)[] filters)
        {
            var dialog = new SaveFileDialog
            {
                OverwritePrompt = true
            };

            SetDefaultProperties(dialog, filters);
            return dialog;
        }
    }
}
