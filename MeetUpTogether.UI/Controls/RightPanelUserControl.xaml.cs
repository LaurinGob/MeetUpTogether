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
    /// Interaktionslogik für RightPanelUserControl.xaml
    /// </summary>
    public partial class RightPanelUserControl : UserControl
    {
        public RightPanelUserControl()
        {
            InitializeComponent();
        }
        private void AddNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewNoteInput.Text))
            {
                NotesList.Items.Add(NewNoteInput.Text.Trim());
                NewNoteInput.Clear();
            }
        }

        private void SaveMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Meeting saved successfully.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
