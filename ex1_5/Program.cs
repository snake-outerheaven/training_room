using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// registra o contexto do banco usando Sqlite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=todos.db"));

// habilita descoberta de endpoints e swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// configura swagger no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// redireciona http para https
app.UseHttpsRedirection();


// CREATE: cria um novo todo
app.MapPost("/todos", async (TodoItem todo, AppDbContext db) =>
{
    db.Todos.Add(todo);                // adiciona o item ao contexto
    await db.SaveChangesAsync();       // persiste no banco
    return Results.Created($"/todos/{todo.Id}", todo); // retorna 201
});


// READ: retorna todos os todos
app.MapGet("/todos", async (AppDbContext db) =>
    await db.Todos.ToListAsync()        // consulta todos os registros
);


// UPDATE: atualiza um todo existente
app.MapPut("/todos/{id}", async (int id, TodoItem inputTodo, AppDbContext db) =>
{
    var todo = await db.Todos.FindAsync(id);   // busca pelo id
    if (todo is null)
        return Results.NotFound();             // 404 se não existir

    todo.TaskName = inputTodo.TaskName;        // atualiza campos
    todo.IsCompleted = inputTodo.IsCompleted;

    await db.SaveChangesAsync();               // salva no banco
    return Results.Ok(todo);                   // retorna item alterado
});


// DELETE: remove um todo existente
app.MapDelete("/todos/{id}", async (int id, AppDbContext db) =>
{
    var todo = await db.Todos.FindAsync(id);   // busca pelo id
    if (todo is null)
        return Results.NotFound();             // 404 se não existir

    db.Todos.Remove(todo);                     // remove do contexto
    await db.SaveChangesAsync();               // salva no banco

    return Results.NoContent();                // 204 sem corpo
});

// Adicione isto em Program.cs, logo antes de app.Run();
// Isso cria uma UI simples em HTML puro (sem JS) que interage com os endpoints via forms.

app.MapGet("/ui", async (AppDbContext db) =>
{
    var todos = await db.Todos.OrderBy(t => t.Id).ToListAsync();            // busca todos do DB
    var html = new System.Text.StringBuilder();                            // monta o HTML no servidor

    html.AppendLine("<!doctype html>");
    html.AppendLine("<html lang=\"pt-BR\">");
    html.AppendLine("<head>");
    html.AppendLine("  <meta charset=\"utf-8\" />");
    html.AppendLine("  <meta name=\"viewport\" content=\"width=device-width,initial-scale=1\" />");
    html.AppendLine("  <title>ToDo - UI</title>");
    html.AppendLine("  <style>body{font-family:Segoe UI,Roboto,Arial;margin:30px} form{margin:8px 0} input[type=text]{width:300px;padding:6px} button{padding:6px 10px;margin-left:6px}</style>");
    html.AppendLine("</head>");
    html.AppendLine("<body>");
    html.AppendLine("  <h1>To-Do List</h1>");

    // Formulário de criação (CREATE)
    html.AppendLine("  <section>");
    html.AppendLine("    <h2>Criar nova tarefa</h2>");
    html.AppendLine("    <form method=\"post\" action=\"/todos/create\">");
    html.AppendLine("      <input type=\"text\" name=\"TaskName\" placeholder=\"Nome da tarefa\" required />");
    html.AppendLine("      <button type=\"submit\">Criar</button>");
    html.AppendLine("    </form>");
    html.AppendLine("  </section>");

    // Lista (READ) com formulários para UPDATE e DELETE por item
    html.AppendLine("  <section>");
    html.AppendLine("    <h2>Lista de tarefas</h2>");
    html.AppendLine("    <ul>");
    foreach (var t in todos)
    {
        html.AppendLine("      <li>");
        html.AppendLine($"        <form method=\"post\" action=\"/todos/{t.Id}/edit\" style=\"display:inline-block;\">");
        html.AppendLine($"          <input type=\"hidden\" name=\"Id\" value=\"{t.Id}\" />");
        html.AppendLine($"          <input type=\"text\" name=\"TaskName\" value=\"{System.Net.WebUtility.HtmlEncode(t.TaskName)}\" required />");
        var checkedAttr = t.IsCompleted ? "checked" : "";
        html.AppendLine($"          <label style=\"margin-left:8px\"><input type=\"checkbox\" name=\"IsCompleted\" {checkedAttr} /> Concluída</label>");
        html.AppendLine("          <button type=\"submit\">Salvar</button>");
        html.AppendLine("        </form>");

        // Form de remoção (DELETE) — usa POST por compatibilidade de forms HTML
        html.AppendLine("        <form method=\"post\" action=\"/todos/" + t.Id + "/delete\" style=\"display:inline-block;margin-left:8px;\">");
        html.AppendLine("          <button type=\"submit\" onclick=\"return confirm('Remover esta tarefa?');\">Remover</button>");
        html.AppendLine("        </form>");

        html.AppendLine("      </li>");
    }
    html.AppendLine("    </ul>");
    html.AppendLine("  </section>");

    html.AppendLine("</body>");
    html.AppendLine("</html>");

    return Results.Content(html.ToString(), "text/html");                         // retorna HTML ao cliente
});

// CREATE handler que recebe form-urlencoded
app.MapPost("/todos/create", async (HttpRequest req, AppDbContext db) =>
{
    var form = await req.ReadFormAsync();                                         // lê dados do form
    var name = form["TaskName"].ToString().Trim();                                // extrai TaskName
    if (string.IsNullOrEmpty(name)) return Results.Redirect("/ui");               // valida mínima

    var todo = new TodoItem { TaskName = name, IsCompleted = false, CreatedAt = DateTime.UtcNow };
    db.Todos.Add(todo);                                                            // adiciona ao contexto
    await db.SaveChangesAsync();                                                   // persiste no DB
    return Results.Redirect("/ui");                                                // volta para a UI
});

// UPDATE handler (recebe via form)
app.MapPost("/todos/{id:int}/edit", async (int id, HttpRequest req, AppDbContext db) =>
{
    var todo = await db.Todos.FindAsync(id);                                       // busca pelo id
    if (todo is null) return Results.NotFound();

    var form = await req.ReadFormAsync();                                         // lê dados do form
    var name = form["TaskName"].ToString().Trim();                                
    var isCompleted = form.ContainsKey("IsCompleted") && form["IsCompleted"] == "on"; // checkbox -> "on" quando marcado

    if (!string.IsNullOrEmpty(name)) todo.TaskName = name;                         // atualiza campos
    todo.IsCompleted = isCompleted;

    await db.SaveChangesAsync();                                                   // salva no DB
    return Results.Redirect("/ui");                                                // volta para a UI
});

// DELETE handler (POST via form)
app.MapPost("/todos/{id:int}/delete", async (int id, AppDbContext db) =>
{
    var todo = await db.Todos.FindAsync(id);                                       // busca pelo id
    if (todo is null) return Results.NotFound();

    db.Todos.Remove(todo);                                                         // remove do contexto
    await db.SaveChangesAsync();                                                   // persiste remoção
    return Results.Redirect("/ui");                                                // volta para a UI
});

app.Run(); // inicia o servidor
