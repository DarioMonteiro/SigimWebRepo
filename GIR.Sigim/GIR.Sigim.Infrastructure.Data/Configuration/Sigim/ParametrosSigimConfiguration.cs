using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class ParametrosSigimConfiguration : EntityTypeConfiguration<ParametrosSigim>
    {
        public ParametrosSigimConfiguration()
        {
            ToTable("Parametros", "Sigim");

            //Dario - Cambalacho
            // Essa tabela não tem chave primaria , assim foi como ela só tem um registro
            // foi escolhido o primeiro campo do tipo int nulo para ser a chave primaria ficticia 
            // para o modelo , para que não ocorra erro, mas no banco ele continua sem chave.
            //
            Ignore(l => l.Id);

            HasKey(l => l.IndiceVendas);

            Property(l => l.IndiceVendas)
                .HasColumnName("indiceVendas")
                .HasColumnOrder(7);

            Property(l => l.MetodoDescapitalizacao)
                .HasColumnName("metodoDescapitalizacao")
                .HasMaxLength(50)
                .HasColumnOrder(10);


            Property(l => l.CorrecaoMesCheioDiaPrimeiro)
                .HasColumnName("correcaoMesCheioDiaPrimeiro")
                .HasColumnOrder(38);
        }
    }
}
