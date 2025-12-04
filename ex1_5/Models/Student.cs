using System.Text.Json.Serialization;

public class Student
{
    public Guid Id { get; }
    public string Name { get; private set; }

    [JsonPropertyName("Grades")]
    private List<float> _grades;

    [JsonIgnore]
    public IReadOnlyList<float> Grades => _grades.AsReadOnly();

    [JsonIgnore]
    public float Average => _grades.Count == 0 ? 0f : _grades.Average();

    public Student(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do aluno não pode ser vazio.");
        Id = Guid.NewGuid();
        Name = name;
        _grades = new List<float>();
    }

    public Student(string name, List<float> grades) : this(name)
    {
        _grades = grades?.Where(g => !float.IsNaN(g) && g >= 0f).ToList() ?? new List<float>();
    }

    [JsonConstructor]
    public Student(Guid id, string name, List<float> grades)
    {
        Id = id;
        Name = name;
        _grades = grades ?? new List<float>();
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome não pode ser vazio.");
        Name = name;
    }

    public void AddGrade(float grade)
    {
        if (float.IsNaN(grade) || grade < 0)
            throw new ArgumentException("Nota inválida!");
        _grades.Add(grade);
    }

    public void AddGrade(params float[] grades)
    {
        if (grades == null) return;
        _grades.AddRange(grades.Where(g => !float.IsNaN(g) && g >= 0f));
    }

    public void RemoveGrade(int index)
    {
        if (index < 0 || index >= _grades.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        _grades.RemoveAt(index);
    }

    public void RemoveGrade(params int[] indices)
    {
        if (indices == null || indices.Length == 0)
            return;
        foreach (var index in indices.OrderByDescending(i => i))
        {
            if (index < 0 || index >= _grades.Count)
                throw new ArgumentOutOfRangeException(nameof(indices), $"Índice inválido: {index}");
            _grades.RemoveAt(index);
        }
    }

    public void SetGrade(float grade, int index)
    {
        if (float.IsNaN(grade) || grade < 0)
            throw new ArgumentException("Nota inválida!");
        if (index < 0 || index >= _grades.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        _grades[index] = grade;
    }

    public void SetGrade(List<float> grades)
    {
        _grades = grades?.Where(g => !float.IsNaN(g) && g >= 0f).ToList() ?? new List<float>();
    }

    public override string ToString()
    {
        string notasStr = Grades.Count == 0
            ? "Nenhuma nota"
            : string.Join(", ", Grades.Select(g => g.ToString("F2")));

        return $"{Name} (Id: {Id}) - Média: {Average:F2}, Notas: [{notasStr}]";
    }
}