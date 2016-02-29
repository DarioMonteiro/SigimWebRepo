using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Entity.CredCob;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Application.DTO.CredCob;

namespace GIR.Sigim.Application.Service.CredCob
{
    public interface ITituloCredCobAppService
    {
        Specification<TituloCredCob> MontarSpecificationMovimentoCredCobRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, int? idUsuario);
        List<TituloDetalheCredCobDTO> RecTit(List<TituloCredCob> listaTituloCredCob, Nullable<DateTime> dataReferencia, bool excluiTabelaTemporaria, bool corrigeParcelaResiduo);
    }
}
