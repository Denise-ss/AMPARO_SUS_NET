using AmparoSus.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AmparoSus.Api.Data;

// DBCONTEXT: faz a ponte entre as classes em C# e as tabelas no Oracle.
public class AmparoSusDbContext : DbContext
{
    public AmparoSusDbContext(DbContextOptions<AmparoSusDbContext> options) : base(options) { }

    public DbSet<Idoso> Idosos => Set<Idoso>();
    public DbSet<SolicitacaoApoio> Solicitacoes => Set<SolicitacaoApoio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tabela 1: AMP_IDOSOS
        modelBuilder.Entity<Idoso>(entidade =>
        {
            entidade.ToTable("AMP_IDOSOS");
            entidade.HasKey(x => x.Id);
            entidade.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            entidade.Property(x => x.Nome).HasColumnName("NOME").HasMaxLength(100).IsRequired();
            entidade.Property(x => x.Telefone).HasColumnName("TELEFONE").HasMaxLength(20).IsRequired();
            entidade.Property(x => x.Cidade).HasColumnName("CIDADE").HasMaxLength(80).IsRequired();
        });

        // Tabela 2: AMP_SOLICITACOES
        modelBuilder.Entity<SolicitacaoApoio>(entidade =>
        {
            entidade.ToTable("AMP_SOLICITACOES");
            entidade.HasKey(x => x.Id);
            entidade.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            entidade.Property(x => x.IdosoId).HasColumnName("IDOSO_ID");
            entidade.Property(x => x.Tipo).HasColumnName("TIPO").HasMaxLength(30).IsRequired();
            entidade.Property(x => x.Descricao).HasColumnName("DESCRICAO").HasMaxLength(250).IsRequired();
            entidade.Property(x => x.Status).HasColumnName("STATUS").HasMaxLength(30).IsRequired();
            entidade.Property(x => x.CriadaEmUtc).HasColumnName("CRIADA_EM_UTC").IsRequired();

            // 1:N. Nesta demonstração com dados fictícios, ao apagar o idoso,
            // suas solicitações também são apagadas (exclusão em cascata).
            entidade.HasOne(x => x.Idoso)
                .WithMany(x => x.Solicitacoes)
                .HasForeignKey(x => x.IdosoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
