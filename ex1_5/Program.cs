class Student
{
    public Guid Id { get; } // id do aluno no sistema
    public string Name { get; private set; } // nome do aluno
    private List<float> _grades; // array privado de notas de cada aluno
    public IReadOnlyList<float> Grades => _grades.AsReadOnly(); // notas encapsuladas

    public float Average => _grades.Average(); // aluno sempre terá a sua média.

    public Student(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do aluno não pode ser vazio ou conter apenas espaços.");

        Id = Guid.NewGuid();
        Name = name;
        _grades = new List<float>();
    }

    /*
        Construtor sobrecarregado para permitir diversas formas de instanciar um estudante no sistema.
    */
    public Student(string name, List<float> grades) : this(name)
    {
        _grades = grades?.Where(g => !float.IsNaN(g) && g >= 0f).ToList() ?? new List<float>();
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do aluno não pode ser vazio ou conter apenas espaços.");
        Name = name;
    }

    public void AddGrade(float grade)
    {
        if (float.IsNaN(grade) || grade < 0)
            throw new ArgumentException("A nota a ser adicionada não pode ser NaN!");
        _grades.Add(grade);
    }

    public void RemoveGrade(int index)
    {
        if (index < 0 || index >= _grades.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Índice inválido!");
        _grades.RemoveAt(index);
    }

    public void SetGrade(float grade, int index)
    {
        if (float.IsNaN(grade) || grade < 0)
            throw new ArgumentException("A nota a ser digitada não pode ser NaN ou negativa!");
        if (index < 0 || index >= _grades.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Índice inválido!");
        _grades[index] = grade;
    }
    public void SetGrade(List<float> grades)
{
    _grades = grades?.Where(g => !float.IsNaN(g) && g >= 0f).ToList() ?? new List<float>();
}


    public override string ToString()
    {
        return $"{Name} (Id: {Id}) - Média: {Average:F2}, Notas: {string.Join(", ", _grades)}";
    }
}
