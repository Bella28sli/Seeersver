using Serveeer.ViewModel;
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

namespace Serveeer.View
{
    /// <summary>
    /// Логика взаимодействия для ChatWindow1xaml.xaml
    /// </summary>
    public partial class ChatWindow1xaml : Window
    {
        public ChatWindow1xaml(bool isServer, MainViewModel mainViewModel)
        {
            InitializeComponent();
            if (isServer)
            {
                ServerViewModel serverViewModel = new ServerViewModel(this, mainViewModel);

                DataContext = serverViewModel;
            }
            else
            {
                LogsButton.IsEnabled = false;
                DataContext = mainViewModel;
            }
        }

    }
}
