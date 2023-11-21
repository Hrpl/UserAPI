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
}