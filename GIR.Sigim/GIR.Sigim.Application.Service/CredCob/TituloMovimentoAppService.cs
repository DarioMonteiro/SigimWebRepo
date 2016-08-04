using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Entity.CredCob;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Domain.Specification.CredCob;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;

namespace GIR.Sigim.Application.Service.CredCob
{
    public class TituloMovimentoAppService : BaseAppService, ITituloMovimentoAppService
    {
        #region Declaração

        private IUsuarioAppService usuarioAppService;
        private IClasseRepository classeRepository;

        #endregion

        #region Construtor

        public TituloMovimentoAppService(IUsuarioAppService usuarioAppService,
                                         IClasseRepository classeRepository,
                                         MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.usuarioAppService = usuarioAppService;
            this.classeRepository = classeRepository;
        }

        #endregion

        #region Métodos ITituloMovimentoAppService

        public Specification<TituloMovimento> MontarSpecificationTituloMovimentoRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, int? idUsuario)
        {
            var specification = (Specification<TituloMovimento>)new TrueSpecification<TituloMovimento>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Financeiro))
            {
                specification &= TituloMovimentoSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Financeiro);
            }
            else
            {
                specification &= TituloMovimentoSpecification.EhCentroCustoAtivo();
            }

            if (filtro.CentroCusto != null)
            {
                specification &= TituloMovimentoSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= TituloMovimentoSpecification.DataPeriodoMaiorOuIgualRelApropriacaoPorClasse(filtro.DataInicial);
            specification &= TituloMovimentoSpecification.DataPeriodoMenorOuIgualRelApropriacaoPorClasse(filtro.DataFinal);

            specification &= TituloMovimentoSpecification.EhSituacaoIgualConferido();
            specification &= TituloMovimentoSpecification.PossuiContaCorrente();

            specification &= TituloMovimentoSpecification.EhTipoParticipanteTitular();

            specification &= TituloMovimentoSpecification.EhValorMovimentoMaiorQueZero();

            if (filtro.ListaClasseReceita.Count > 0)
            {
                string[] arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseReceita);

                if (arrayCodigoClasse.Length > 0)
                {
                    specification &= TituloMovimentoSpecification.SaoClassesExistentes(arrayCodigoClasse);
                }
            }

            return specification;
        }

        #endregion

        #region "Métodos privados"

        private string[] PopulaArrayComCodigosDeClassesSelecionadas(List<ClasseDTO> listaClasses)
        {
            List<Classe> listaClassesSelecionadas = new List<Classe>();

            foreach (var classeSelecionada in listaClasses)
            {
                var classe = classeRepository.ObterPeloCodigo(classeSelecionada.Codigo, l => l.ListaFilhos);

                MontaArrayClasseExistentes(classe, listaClassesSelecionadas);
            }

            int i = 0;
            string[] arrayCodigoClasse = new string[listaClassesSelecionadas.Count()];
            foreach (var classe in listaClassesSelecionadas)
            {
                arrayCodigoClasse[i] = classe.Codigo;
                i += 1;
            }

            return arrayCodigoClasse;

        }

        private void MontaArrayClasseExistentes(Classe classeSelecionada, List<Classe> listaClassesSelecionadas)
        {
            if (classeSelecionada.ListaFilhos.Count == 0)
            {
                if (!listaClassesSelecionadas.Any(l => l.Codigo == classeSelecionada.Codigo))
                {
                    listaClassesSelecionadas.Add(classeSelecionada);
                }
            }
            else
            {
                foreach (var classeFilha in classeSelecionada.ListaFilhos)
                {
                    MontaArrayClasseExistentes(classeFilha, listaClassesSelecionadas);
                }
            }
        }

        #endregion

    }
}
