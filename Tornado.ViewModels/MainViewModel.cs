using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Tornado.Business;

namespace Tornado.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IMainView _mainView;

        private string _cleanupResult;
        private string _path;

        private ICommand _cleanupCommand;
        private RelayCommand _closeCommand;

        private readonly PluginManager _pluginManager = new PluginManager();
        private readonly CleanUpTask _cleanUpTask = new CleanUpTask();

        public MainViewModel(IMainView mainView)
        {
            _mainView = mainView;
        }

        /// <summary>
        /// Load all plugins at startup
        /// </summary>
        public async Task LoadPlugins()
        {
            await _pluginManager.LoadAsync(System.AppDomain.CurrentDomain.BaseDirectory);
        }

        /// <summary>
        /// Display a message after the job is completed
        /// </summary>
        public string CleanupResult
        {
            get { return _cleanupResult; }
            set
            {
                _cleanupResult = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Path to cleanup
        /// </summary>
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Close the app
        /// </summary>
        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(DoClose)); }
        }


        /// <summary>
        /// Command that do the job
        /// </summary>
        public ICommand CleanupCommand
        {
            get { return _cleanupCommand ?? (_cleanupCommand = new RelayCommand(DoCleanup)); }
        }

        /// <summary>
        /// Do the job
        /// </summary>
        private async void DoCleanup()
        {
            if (_cleanUpTask.IsRunning)
                return;

            await _cleanUpTask.CleanUp(Path);

            CleanupResult = "Clean up completed";
        }


        /// <summary>
        /// Do close the app
        /// </summary>
        private void DoClose()
        {
            _mainView.Close();
        }
    }
}
