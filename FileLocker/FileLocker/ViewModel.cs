using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace FileLocker
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> List { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand ReleaseCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        List<FileStream> streams;

        public ViewModel()
        {
            List = new ObservableCollection<string>();
            streams = new List<FileStream>();

            AddCommand = new DelegateCommand((parameter) =>
            {
                var dialog = new OpenFileDialog();

                dialog.Multiselect = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var fileName in dialog.FileNames)
                    {
                        List.Add(fileName);
                        streams.Add(new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None));
                    }
                }
            });

            ReleaseCommand = new DelegateCommand((parameter) =>
            {
                foreach(var stream in streams)
                {
                    try
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                    catch { }
                }
            });
        }
    }
}