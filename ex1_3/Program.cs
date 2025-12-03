/*
    Exemplo de como a linguagem suporta concatenação de strings com o operador + 
*/
string name1 = "Bob";
string name2 = "Daniel";
string output = "Bem vindos " + name1 + " e " + name2 + "!";
Console.WriteLine(output);

// o próximo exercício é somente concatenação de strings dentro do Console.WriteLine()
Console.WriteLine(name1 + " and " + name2 + ", " + "Greetings!");


/*
    Agora começa a ficar interessante, $"{var1} {var2}" é como C# permite a interpolação de strings
*/

var output2 = $"Bem vindos {name1} e {name2}!"; // var para inferência de tipo pelo Roslyn
Console.WriteLine(output2);

/*
    Combinando literais e strings interpoladas
*/ 

var output3 = $@"C:\Users\{name1}> ";
Console.WriteLine(output3);

/*
    Desafio -> Obter a saída : 
"    
View English output:
  c:\Exercise\ACME\data.txt

Посмотреть русский вывод:
  c:\Exercise\ACME\ru-RU\data.txt

"
com as variáveis fornecidas abaixo.
*/

string projectName = "ACME";

string russianMessage = "\u041f\u043e\u0441\u043c\u043e\u0442\u0440\u0435\u0442\u044c \u0440\u0443\u0441\u0441\u043a\u0438\u0439 \u0432\u044b\u0432\u043e\u0434";


Console.WriteLine($@"
View English Output: 
    C:\Exercise\{projectName}\data.txt
{russianMessage}:
    C:\Exercise\{projectName}\ru-RU\data.txt
");