using MinimalApiProject.Enums;

namespace MinimalApiProject.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataConclusao { get; set; }
        public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
