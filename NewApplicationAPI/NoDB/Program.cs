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
    // �������� ������������ �� id
    User? user = users.FirstOrDefault(u => u.Id == id);
    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null) return Results.NotFound(new { message = "������������ �� ������" });

    // ���� ������������ ������, ���������� ���
    return Results.Json(user);
});

app.MapDelete("/api/users/{id}", (string id) =>
{
    // �������� ������������ �� id
    User? user = users.FirstOrDefault(u => u.Id == id);

    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null) return Results.NotFound(new { message = "������������ �� ������" });

    // ���� ������������ ������, ������� ���
    users.Remove(user);
    return Results.Json(user);
});

app.MapPost("/api/users", (User user) => {

    // ������������� id ��� ������ ������������
    user.Id = Guid.NewGuid().ToString();
    // ��������� ������������ � ������
    users.Add(user);
    return user;
});

app.MapPut("/api/users", (User userData) => {

    // �������� ������������ �� id
    var user = users.FirstOrDefault(u => u.Id == userData.Id);
    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null) return Results.NotFound(new { message = "������������ �� ������" });
    // ���� ������������ ������, �������� ��� ������ � ���������� ������� �������

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