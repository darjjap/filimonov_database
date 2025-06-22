using project.fil.Classes;
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

namespace project.fil.Pages
{
    /// <summary>
    /// Логика взаимодействия для SignInPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
        public SignInPage()
        {
            InitializeComponent();
        }
        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            var CurrentUser = AppData.db.User.FirstOrDefault(
                u => u.Login == TxbLogin.Text && u.Password == TxbPassword.Text
            );

            if (CurrentUser == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            MainWindow mainWindow = new MainWindow();

            Application.Current.MainWindow = mainWindow;

            mainWindow.Show();

            Window.GetWindow(this)?.Close();
        }
    }
}
