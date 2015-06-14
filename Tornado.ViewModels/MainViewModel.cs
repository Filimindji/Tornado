using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Tornado.Business;

namespace Tornado.ViewModels
{
    public interface IMainView
    {
        void Close();
    }

    public class MainViewModel : ViewModelBase
    {
        private readonly IMainView _mainView;
        private RelayCommand _closeCommand;
        private string _cleanupResult;
        private string _path;
        private ICommand _cleanupCommand;

        public MainViewModel(IMainView mainView)
        {
            _mainView = mainView;
        }

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(DoClose)); }
        }

        public string CleanupResult
        {
            get { return _cleanupResult; }
            set
            {
                _cleanupResult = value;
                RaisePropertyChanged();
            }
        }

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CleanupCommand
        {
            get { return _cleanupCommand ?? (_cleanupCommand = new RelayCommand(DoCleanup)); }
        }

        private async void DoCleanup()
        {
            CleanUpTask cleanUpTask = new CleanUpTask();
            await cleanUpTask.CleanUp(Path);
            CleanupResult = "Completed";
        }

        private void DoClose()
        {
            _mainView.Close();
        }
    }
}
