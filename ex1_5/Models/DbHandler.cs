using System.Text.Json;

public class DbHandler
{
    /*
        Tudo que é static será compartilhado entre todas as instâncias de DbHander.
    */

    private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "students.json");

    private static List<Student> Students = new List<Student>();

    public static IReadOnlyList<Student> StudentsPeek => Students.AsReadOnly();

    public static void Load()
    {
        if (!File.Exists(DbPath))
        {
            File.WriteAllText(DbPath, "[]");
            return;
        }
        string json = File.ReadAllText(DbPath);
        if (string.IsNullOrWhiteSpace(json))
            return;
        try
        {
            List<Student> dbStudents = JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
            Students.Clear();
            Students.AddRange(dbStudents);

        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Erro ao carregar do banco de dados(estrutura vazia ou corrompida): {ex.Message}");
            Thread.Sleep(750);
            Console.WriteLine("Iniciando limpeza do arquivo do banco de dados.");
            Students.Clear();
            File.WriteAllText(DbPath, "[]");
            Console.WriteLine("Arquivo do db limpo para novas operações!");
            return;
        }
    }

};
