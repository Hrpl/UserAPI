# Получение имя пользователя через его Id, с помощью WebApi

## Листинг API <br>
```C#
List<User> users = new List<User>
{
    new() { Id = "1", Name = "Tom", Password = "37" },
    new() { Id = "2", Name = "Bob", Password = "41" },
    new() { Id = "3", Name = "Sam", Password = "24"}
};

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/users", () => users);

app.MapGet("/api/users/{id}", (string id) =>
{
    // получаем пользователя по id
    User? user = users.FirstOrDefault(u => u.Id == id);
    // если не найден, отправляем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });

    // если пользователь найден, отправляем его
    return Results.Json(user);
});

app.MapDelete("/api/users/{id}", (string id) =>
{
    // получаем пользователя по id
    User? user = users.FirstOrDefault(u => u.Id == id);

    // если не найден, отправляем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });

    // если пользователь найден, удаляем его
    users.Remove(user);
    return Results.Json(user);
});

app.MapPost("/api/users", (User user) => {

    // устанавливаем id для нового пользователя
    user.Id = Guid.NewGuid().ToString();
    // добавляем пользователя в список
    users.Add(user);
    return user;
});

app.MapPut("/api/users", (User userData) => {

    // получаем пользователя по id
    var user = users.FirstOrDefault(u => u.Id == userData.Id);
    // если не найден, отправляем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });
    // если пользователь найден, изменяем его данные и отправляем обратно клиенту

    user.Password = userData.Password;
    user.Name = userData.Name;
    return Results.Json(user);
});

app.Run();

public class User
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Password { get; set; }
}``` <br>

## Листинг WPF
```C#
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
}```

## Модель данных
```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewApp2.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
```