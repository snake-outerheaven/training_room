using System.Text.Json;

public class DbHandler
{
    /*
        Tudo que é static será compartilhado entre todas as instâncias de DbHander.
    */

    private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "students.json");

    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true };

    private static List<Student> Students = new List<Student>();

    public static IReadOnlyList<Student> StudentsPeek => Students.AsReadOnly();

    private static void Load()
    {
        try
        {
            if (!File.Exists(DbPath))
            {
                File.WriteAllText(DbPath, "[]");
                return;
            }
        }
        catch (IOException ioex)
        {
            Console.WriteLine($"Erro de I/O ao verificar/criar o arquivo: {ioex.Message}");
            File.AppendAllText("error.log",
                $"[{DateTime.Now}] I/O error on DB existence check: {ioex.Message}\n");
            return;
        }

        string json;

        try
        {
            json = File.ReadAllText(DbPath);
        }
        catch (IOException ioex)
        {
            Console.WriteLine($"Erro de I/O ao ler o arquivo: {ioex.Message}");
            File.AppendAllText("error.log",
                $"[{DateTime.Now}] I/O error on DB read: {ioex.Message}\n");
            return;
        }

        if (string.IsNullOrWhiteSpace(json))
            return;

        try
        {
            List<Student> dbStudents =
                JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();

            Students.Clear();
            Students.AddRange(dbStudents);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Erro ao interpretar JSON: {ex.Message}");
            Thread.Sleep(750);
            Students.Clear();
            File.WriteAllText(DbPath, "[]");
        }
    }


    private static void Save()
    {
        string json;
        try
        {
            json = JsonSerializer.Serialize(Students, Options);
            if (string.IsNullOrWhiteSpace(json))
                json = "[]";
        }
        catch (JsonException ex)
        {
            Thread.Sleep(750);
            Console.WriteLine($"Erro ao serializar o estado em JSON.\nMensagem: {ex.Message}");
            File.AppendAllText("error.log",
                $"[{DateTime.Now}] JSON serialization failure: {ex.Message}\n");
            return;
        }

        try
        {
            string tempFile = DbPath + ".tmp";
            File.WriteAllText(tempFile, json);
            File.Move(tempFile, DbPath, overwrite: true);
        }
        catch (IOException ioex)
        {
            Thread.Sleep(750);
            Console.WriteLine($"Erro de I/O ao salvar o banco.\nMensagem: {ioex.Message}");
            File.AppendAllText("error.log",
                $"[{DateTime.Now}] I/O failure while writing DB: {ioex.Message}\n");
        }
    }

};
