using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeetUpTogether.UI.Controls
{
    /// <summary>
    /// Interaktionslogik für SearchBarUserControl.xaml
    /// </summary>
    public partial class SearchBarUserControl : UserControl
    {
        public SearchBarUserControl()
        {
            InitializeComponent();
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSearch();
        }

        private void SearchInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ExecuteSearch();
        }

        private void ExecuteSearch()
        {
            string query = SearchInput.Text.Trim();
            if (!string.IsNullOrEmpty(query))
            {
                MessageBox.Show($"Searching for: {query}", "Search", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
