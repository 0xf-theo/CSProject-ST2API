using Project.Chat.Common.Packets;
using Project.Chat.viewModels;
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
using System.Windows.Shapes;

namespace Project.Chat.modals
{
    /// <summary>
    /// Logique d'interaction pour CreateTopicModal.xaml
    /// </summary>
    public partial class CreateTopicModal : Window
    {
        public CreateTopicModal()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as CreateTopicViewModel;
            if (viewModel != null)
            {
                App.SocketClient.SendPacket(new CreateTopicPacket()
                {
                    Name = viewModel.TopicName
                });

                this.Close();
            }
        }
    }
}
