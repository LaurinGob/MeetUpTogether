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
    /// Interaktionslogik für LeftPanelUserControl.xaml
    /// </summary>
    public partial class LeftPanelUserControl : UserControl
    {
        public LeftPanelUserControl()
        {
            InitializeComponent();
        }
        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            ItemsList.Items.Add($"Item {ItemsList.Items.Count + 1}");
        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsList.SelectedItem != null)
            {
                ItemsList.Items.Remove(ItemsList.SelectedItem);
            }
            else
            {
                MessageBox.Show("Please select an item to remove.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
