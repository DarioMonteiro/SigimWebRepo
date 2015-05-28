using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.Contrato
{
    public class ContratoRepository : Repository<Domain.Entity.Contrato.Contrato>, IContratoRepository
    {
        #region Contructor

        public ContratoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<TEntity> Members

        public override IEnumerable<Domain.Entity.Contrato.Contrato> ListarPeloFiltroComPaginacao(
            ISpecification<Domain.Entity.Contrato.Contrato> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<Domain.Entity.Contrato.Contrato, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "centroCusto":
                    set = ascending ? set.OrderBy(l => l.CentroCusto.Codigo) : set.OrderByDescending(l => l.CentroCusto.Codigo);
                    break;
                case "descricaoContrato":
                    set = ascending ? set.OrderBy(l => l.ContratoDescricao.Descricao) : set.OrderByDescending(l => l.ContratoDescricao.Descricao);
                    break;
                case "contratante":
                    set = ascending ? set.OrderBy(l => l.Contratante.Nome) : set.OrderByDescending(l => l.Contratante.Nome);
                    break;
                case "contratado":
                    set = ascending ? set.OrderBy(l => l.Contratado.Nome) : set.OrderByDescending(l => l.Contratado.Nome);
                    break;
                case "dataAssinatura":
                    set = ascending ? set.OrderBy(l => l.DataAssinatura) : set.OrderByDescending(l => l.DataAssinatura);
                    break;
                case "situacao":
                    set = ascending ? set.OrderBy(l => l.Situacao) : set.OrderByDescending(l => l.Situacao);
                    break;
                case "id":
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        //public IEnumerable<Domain.Entity.Contrato.Contrato> ListarContratosPorAtributos(    string situacao,
        //                                                                                    string dataInicial,
        //                                                                                    string dataFinal,
        //                                                                                    string centroCusto,
        //                                                                                    string contratoDescricao,
        //                                                                                    string contratante,
        //                                                                                    string contratado,
        //                                                                                    string codigo,
        //                                                                                    string tipoContrato)
        //{
        //    short sit;
        //    DateTime datIni,datFim;
        //    int contratoDescricaoId;
        //    int contratanteId;
        //    int contratadoId;
        //    int id;
        //    int tipoContr;

        //    var set = CreateSetAsQueryable();

        //    if (situacao != "")
        //    { 
        //        sit = short.Parse(situacao);
        //        switch(sit)
        //        {
        //            case (short) SituacaoContrato.AguardandoAssinatura : 
        //                set = set.Where(l => l.Situacao == SituacaoContrato.AguardandoAssinatura);
        //                break;
        //            case (short) SituacaoContrato.Assinado : 
        //                set = set.Where(l => l.Situacao == SituacaoContrato.Assinado);
        //                break;
        //            case (short) SituacaoContrato.Cancelado : 
        //                set = set.Where(l => l.Situacao == SituacaoContrato.Cancelado);
        //                break;
        //            case (short) SituacaoContrato.Concluido : 
        //                set = set.Where(l => l.Situacao == SituacaoContrato.Concluido);
        //                break;
        //            case (short) SituacaoContrato.Minuta : 
        //                set = set.Where(l => l.Situacao == SituacaoContrato.Minuta);
        //                break;
        //            case (short) SituacaoContrato.Retificacao : 
        //                set = set.Where(l => l.Situacao == SituacaoContrato.Retificacao);
        //                break;
        //            case (short) SituacaoContrato.Suspenso : 
        //                set = set.Where(l => l.Situacao == SituacaoContrato.Suspenso);
        //                break;
        //        }

        //    }

        //    if (dataInicial != "")
        //    {
        //        datIni = DateTime.Parse(dataInicial);
        //        set = set.Where(l => l.DataAssinatura >= datIni);
        //    }

        //    if (dataFinal != "")
        //    {
        //        datFim = DateTime.Parse(dataFinal);
        //        set = set.Where(l => l.DataAssinatura <= datFim);
        //    }

        //    if (centroCusto != "")
        //    {
        //        set = set.Where(l => l.CentroCusto.Codigo.ToUpper().Contains(centroCusto.ToUpper()));   
        //    }

        //    if (contratoDescricao != "")
        //    {
        //        contratoDescricaoId = int.Parse(contratoDescricao);
        //        set = set.Where(l => l.ContratoDescricaoId == contratoDescricaoId); 
        //    }

        //    if (contratante != "")
        //    {
        //        contratanteId = int.Parse(contratante);
        //        set = set.Where(l => l.ContratanteId == contratanteId);
        //    }

        //    if (contratado != "")
        //    {
        //        contratadoId = int.Parse(contratado);
        //        set = set.Where(l => l.ContratadoId == contratadoId);
        //    }

        //    if (codigo != "")
        //    {
        //        id = int.Parse(codigo);
        //        set = set.Where(l => l.Id == id);
        //    }

        //    if (tipoContrato != "")
        //    {
        //        tipoContr = int.Parse(tipoContrato);
        //        set = set.Where(l => l.TipoContrato == tipoContr);
        //    }

        //    return set;

        //}

        #endregion
    }
}
