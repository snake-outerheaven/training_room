/*
    Operator overloading, somando tipos diferentes, além do uso de 
    () para controlar ordem de operações.
*/

string a = "bob";
decimal b = 13.0m;
Console.WriteLine(a + " sold " + (b + 7m) + " Bs.");

/*
    Operações matemáticas não são muito diferentes de C, pelo menos
    por enquanto
*/

int sum = 7 + 5;
int difference = 7 - 5;
int product = 7 * 5;
int quotient = 7 / 5;
float floatQuotient = 7.0F / 5.0F;
double doubleQuotient = 7.0d / 5.0d;
decimal decimalQuotient = 7.0m / 5.0m;

Console.WriteLine($"Sum: {sum}.");
Console.WriteLine($"Diff: {difference}.");
Console.WriteLine($"Product: {product}.");
Console.WriteLine($"Quotient: {quotient}.");
Console.WriteLine($"Division with single precision float type number: {floatQuotient}.");
Console.WriteLine($"Division with double precision float point number: {doubleQuotient}.");
Console.WriteLine($"Division with a float point number from the decimal system: {decimalQuotient}.");

/*
    Agora é o type casting padrão
*/

int ab = 1;
int bc = 2;
decimal de = (decimal)ab / (decimal)bc;
Console.WriteLine(de);

/*
    Operações com módulo.
*/

int abc = 7 % 3;
Console.WriteLine("Operações com módulo\t" + abc + "\t->\t" + $"{100 % 3}");

/*
    PEMDAS

    Parênteses -> Exponentes -> Multiplicação e Divisão -> Adição e Subtração.
*/

int value1 = 3 + 4 * 5;
int value2 = (3 + 4) * 5;
Console.WriteLine(value1);
Console.WriteLine(value2);

/*
    C# também possui +=, -=, *=, ++,--, e eles funcionam igual C
*/
int value = 1;

value = value + 1;
Console.WriteLine("First increment: " + value);

value += 1;
Console.WriteLine("Second increment: " + value);

value++;
Console.WriteLine("Third increment: " + value);

value = value - 1;
Console.WriteLine("First decrement: " + value);

value -= 1;
Console.WriteLine("Second decrement: " + value);

value--;
Console.WriteLine("Third decrement: " + value);

// PS, existe pré e pós incremento também!!

/*
    Desafio para calcular celsius dada temperatura em fahrenheit.
*/

int fahrenheit = 94;
decimal celsius = (fahrenheit - 32m) * (5m / 9m);
Console.WriteLine(celsius.ToString("F2"));