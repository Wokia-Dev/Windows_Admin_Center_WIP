using Windows_Admin_Center.Core;

namespace Windows_Admin_Center.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand GeneralViewCommand { get; set; }

        public RelayCommand OptimizationViewCommand { get; set; }

        public RelayCommand CustomizationViewCommand { get; set; }

        public RelayCommand SecurityViewCommand { get; set; }

        public RelayCommand NetworkViewCommand { get; set; }

        public RelayCommand OtherViewCommand { get; set; }


        public GeneralViewModel GeneralVm { get; set; }

        public OptimizationViewModel OptimizationVm { get; set; }

        public CustomizationViewModel CustomizationVm { get; set; }

        public SecurityViewModel SecurityVm { get; set; }

        public NetworkViewModel NetworkVm { get; set; }

        public OtherViewModel OtherVm { get; set; }


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            GeneralVm = new GeneralViewModel();
            OptimizationVm = new OptimizationViewModel();
            CustomizationVm = new CustomizationViewModel();
            SecurityVm = new SecurityViewModel();
            NetworkVm = new NetworkViewModel();
            OtherVm = new OtherViewModel();

            CurrentView = GeneralVm;

            GeneralViewCommand = new RelayCommand(o =>
            {
                CurrentView = GeneralVm;
            });

            OptimizationViewCommand = new RelayCommand(o =>
            {
                CurrentView = OptimizationVm;
            });

            CustomizationViewCommand = new RelayCommand(o =>
            {
                CurrentView = CustomizationVm;
            });

            SecurityViewCommand = new RelayCommand(o =>
            {
                CurrentView = SecurityVm;
            });

            NetworkViewCommand = new RelayCommand(o =>
            {
                CurrentView = NetworkVm;
            });

            OtherViewCommand = new RelayCommand(o =>
            {
                CurrentView = OtherVm;
            });


        }
    }
}
