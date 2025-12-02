/*
    Sequências de escape são iguais em C/C++
*/

Console.WriteLine("Hello\nWorld!");
Console.WriteLine("Hello\tWorld!");


/*
    Exemplinho top, e explicando como @ pode ser usado para ignorar sequências de escape e fazer a string
    dentro de WriteLine e provavelmente Write aparecer no terminal da mesma forma que foram escritas 
    no código.
*/

Console.WriteLine("Generating invoices for customer \"Contoso Corpo\" ...\n");
Console.WriteLine("Invoice: 1021\t\tComplete!");
Console.WriteLine("Invoide: 1022\t\tComplete!");
Console.Write("\nOutput Directory:\t");
Console.WriteLine(@"C:\sources.");
Console.Write("\n\n\u65e5\u672c\u306e\u8acb\u6c42\u66f8\u3092\u751f\u6210\u3059\u308b\u306b\u306f\uff1a\n\t");
Console.WriteLine(@"c:\invoices\app.exe -j");

/*
    Exemplo extremo
*/
Console.WriteLine(@"
a
b
c
d
e
f
g
");

/*
    Como escrever caracteres unicode, hello world em Japonês

    "Sekai yo, ore ga kita ze!"
    em português seria: 
    "Mundo, cheguei, caramba!"
*/

Console.WriteLine("\u4E16\u754C\u3088\u3001\u4FFA\u304C\u6765\u305F\u305C\uFF01");