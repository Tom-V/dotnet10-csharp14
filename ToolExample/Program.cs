using Microsoft.Data.Sqlite;
using Spectre.Console;

using var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();

var command = connection.CreateCommand();
command.CommandText = "SELECT 'world'";

using var reader = command.ExecuteReader();
while (reader.Read())
{
    var name = reader.GetString(0);

    var figlet = new FigletText($"Hello {name}!")
        .Centered()
        .Color(Color.Green);

    AnsiConsole.Write(figlet);
}
