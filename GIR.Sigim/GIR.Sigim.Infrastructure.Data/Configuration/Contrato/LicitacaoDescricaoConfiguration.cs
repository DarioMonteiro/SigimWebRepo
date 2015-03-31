using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Contrato
{
    
    public class LicitacaoDescricaoConfiguration : EntityTypeConfiguration<LicitacaoDescricao>
    {
        public LicitacaoDescricaoConfiguration()
        {
            ToTable("Contrato", "Licitacao");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);           

            Property(l => l.Descricao)
               .HasColumnName("descricao")
               .HasColumnOrder(2);

        }
    }
}
