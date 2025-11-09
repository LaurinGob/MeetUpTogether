namespace MeetUpTogether.UI.ViewModels
{
    public class SearchBarViewModel : BaseViewModel
    {
        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(); }
        }
    }
}
