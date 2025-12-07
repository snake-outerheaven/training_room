using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data;

public class AppDbContext : DbContext
{
        public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options) { }                 // injeta as opções de configuração do contexto
        
        public DbSet<TodoItem> Todos => Set<TodoItem>(); // representa a tabela Todos no banco
}
