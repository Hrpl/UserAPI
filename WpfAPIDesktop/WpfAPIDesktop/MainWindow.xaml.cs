using NewApp2.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAPIDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetUserNameAsync();
        }

        private async void SetUserNameAsync()
        {
            var user = await GetUserName();
            if (user != null)
            {
                LabelForName.Text += user.Name.ToString();
            }
        }

        static async Task<User?> GetUserName()
        {
            var sock = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10),
            };

            using HttpClient client = new HttpClient(sock);

            HttpResponseMessage response = await client.GetAsync("https://localhost:7154/api/users/4");
            if (response.IsSuccessStatusCode)
            {
                var user = await client.GetFromJsonAsync<User>("https://localhost:7154/api/users/4");
                return user;
            }
            else
            {
                throw new Exception("Error retrieving user data");
            }
        }
    }
}