using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Domain.Specification.Comercial
{
    public class VendaSpecification : BaseSpecification<Venda>
    {
        public static Specification<Venda> IgualAoIncorporadorId(int? incorporadorId)
        {
            Specification<Venda> specification = new TrueSpecification<Venda>();

            if (incorporadorId.HasValue)
            {
                var directSpecification = new DirectSpecification<Venda>(l => l.Contrato.Unidade.Empreendimento.IncorporadorId == incorporadorId);
                specification &= directSpecification;
            }
            return specification;
        }

        public static Specification<Venda> IgualAoEmpreendimentoId(int? empreendimentoId)
        {
            Specification<Venda> specification = new TrueSpecification<Venda>();

            if (empreendimentoId.HasValue)
            {
                var directSpecification = new DirectSpecification<Venda>(l => l.Contrato.Unidade.EmpreendimentoId == empreendimentoId);
                specification &= directSpecification;
            }
            return specification;
        }

        public static Specification<Venda> EhTipoParticipanteTitular()
        {
            Specification<Venda> specification = new TrueSpecification<Venda>();
            return new DirectSpecification<Venda>(l => l.Contrato.ListaVendaParticipante.Any(t => t.TipoParticipanteId == 1)); 
        }

        public static Specification<Venda> IgualAoBlocoId(int? blocoId)
        {
            Specification<Venda> specification = new TrueSpecification<Venda>();

            if (blocoId.HasValue)
            {
                var directSpecification = new DirectSpecification<Venda>(l => l.Contrato.Unidade.BlocoId == blocoId);
                specification &= directSpecification;
            }
            return specification;
        }

        public static Specification<Venda> EhAprovado()
        {
            Specification<Venda> specification = new TrueSpecification<Venda>();
            return new DirectSpecification<Venda>(l => (l.Aprovado.HasValue && l.Aprovado.Value == true)); 
        }

        public static Specification<Venda> NaoEhAprovado()
        {
            Specification<Venda> specification = new TrueSpecification<Venda>();
            return new DirectSpecification<Venda>(l => ((!l.Aprovado.HasValue) || (l.Aprovado.HasValue && l.Aprovado.Value == false)));
        }

        public static Specification<Venda> EhProposta()
        {
            return new DirectSpecification<Venda>(l => l.Contrato.SituacaoContrato == GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoPropostaCodigo);
        }

        public static Specification<Venda> EhAssinada()
        {
            return new DirectSpecification<Venda>(l => l.Contrato.SituacaoContrato == GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoAssinadoCodigo);
        }

        public static Specification<Venda> EhCancelada()
        {
            return new DirectSpecification<Venda>(l => l.Contrato.SituacaoContrato == GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoCanceladoCodigo);
        }

        public static Specification<Venda> EhRescindida()
        {
            return new DirectSpecification<Venda>(l => l.Contrato.SituacaoContrato == GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoRescindidoCodigo);
        }

        public static Specification<Venda> EhQuitada()
        {
            return new DirectSpecification<Venda>(l => l.Contrato.SituacaoContrato == GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoQuitadoCodigo);
        }

        public static Specification<Venda> EhEscriturada()
        {
            return new DirectSpecification<Venda>(l => l.Contrato.SituacaoContrato == GIR.Sigim.Domain.Constantes.Comercial.ContratoSituacaoEscrituradoCodigo);
        }

    }
}