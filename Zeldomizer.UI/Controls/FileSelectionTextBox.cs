using System.Windows.Forms;
using Zeldomizer.UI.Core;

namespace Zeldomizer.UI.Controls
{
    public partial class FileSelectionTextBox : UserControl
    {
        private readonly IFileDialogFactory _fileDialogFactory;

        public FileSelectionTextBox(IFileDialogFactory fileDialogFactory)
        {
            InitializeComponent();
            _fileDialogFactory = fileDialogFactory;
        }

        public override string Text
        {
            get { return fileNameTextBox.Text; }
            set { fileNameTextBox.Text = value; }
        }

        public bool IsSave { get; set; }

        private void LocateFileButtonClicked(object sender, System.EventArgs e)
        {
            var dialog = IsSave
                ? (FileDialog)_fileDialogFactory.GetOpen()
                : _fileDialogFactory.GetSave();

            if (dialog.ShowDialog() == DialogResult.OK)
                Text = dialog.FileName;
        }
    }
}
