using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BillNest.Models;

namespace BillNest.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Client> _clients;

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            Clients = new ObservableCollection<Client>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}