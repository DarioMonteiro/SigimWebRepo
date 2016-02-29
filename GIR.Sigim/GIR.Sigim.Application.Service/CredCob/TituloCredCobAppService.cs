﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.CredCob;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Domain.Entity.CredCob;
using GIR.Sigim.Domain.Repository.CredCob;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Domain.Specification.CredCob;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.CredCob;
using GIR.Sigim.Domain.Repository.Comercial;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Application.Service.CredCob
{
    public class TituloCredCobAppService : BaseAppService, ITituloCredCobAppService
    {
        #region Declaração

        private IUsuarioAppService usuarioAppService;
        private ITituloCredCobRepository tituloCredCobRepository;
        private IClasseRepository classeRepository;
        private IVendaSerieRepository vendaSerieRepository;

        #endregion

        #region Construtor

        public TituloCredCobAppService(IUsuarioAppService usuarioAppService,
                                       ITituloCredCobRepository tituloCredCobRepository,
                                       IClasseRepository classeRepository, 
                                       IVendaSerieRepository vendaSerieRepository,
                                       MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tituloCredCobRepository = tituloCredCobRepository;
            this.usuarioAppService = usuarioAppService;
            this.classeRepository = classeRepository;
            this.vendaSerieRepository = vendaSerieRepository;
        }

        #endregion

        #region Métodos ITituloCredCobAppService

        public Specification<TituloCredCob> MontarSpecificationMovimentoCredCobRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, int? idUsuario)
        {
            var specification = (Specification<TituloCredCob>)new TrueSpecification<TituloCredCob>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Financeiro))
            {
                specification &= TituloCredCobSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Financeiro);
            }
            else
            {
                specification &= TituloCredCobSpecification.EhCentroCustoAtivo();
            }

            if (filtro.CentroCusto != null)
            {
                specification &= TituloCredCobSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= TituloCredCobSpecification.EhTipoParticipanteTitular();
            specification &= TituloCredCobSpecification.EhSituacaoDiferenteDeCancelado();

            specification &= TituloCredCobSpecification.DataPeriodoMaiorOuIgualRelApropriacaoPorClasse(filtro.DataInicial);
            specification &= TituloCredCobSpecification.DataPeriodoMenorOuIgualRelApropriacaoPorClasse(filtro.DataFinal);

            if (filtro.OpcoesRelatorio.HasValue)
            {
                //if (filtro.OpcoesRelatorio.Value != (int)OpcoesRelatorioApropriacaoPorClasse.Sintetico)

                if (filtro.OpcoesRelatorio.Value == (int)OpcoesRelatorioApropriacaoPorClasse.Analitico)
                {
                    if (filtro.ListaClasseReceita.Count > 0)
                    {
                        string[] arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseDespesa);

                        if (arrayCodigoClasse.Length > 0)
                        {
                            specification &= TituloCredCobSpecification.SaoClassesExistentes(arrayCodigoClasse);
                        }
                    }
                }
            }

            if (!(filtro.EhSituacaoAReceberFaturado && filtro.EhSituacaoAReceberRecebido))
            {
                if (filtro.EhSituacaoAReceberFaturado)
                {
                    specification &= TituloCredCobSpecification.EhSituacaoPendente();

                }

                if (filtro.EhSituacaoAReceberRecebido)
                {
                    specification &= TituloCredCobSpecification.EhSituacaoQuitado();
                }
            }


            return specification;
        }

        public List<TituloDetalheCredCobDTO> RecTit(List<TituloCredCob> listaTituloCredCob,
                                                 Nullable<DateTime> dataReferencia,
                                                 bool excluiTabelaTemporaria = false,
                                                 bool corrigeParcelaResiduo = false)
        {
            List<TituloDetalheCredCobDTO> listaTituloDetalheCredCob = new List<TituloDetalheCredCobDTO>();

            foreach (TituloCredCob titulo in listaTituloCredCob)
            {
                TituloDetalheCredCobDTO tituloDetalhe = new TituloDetalheCredCobDTO();

                tituloDetalhe = titulo.To<TituloDetalheCredCobDTO>();

                VendaSerie vendaSerie = vendaSerieRepository.ObterPeloIdComposto(titulo.ContratoId, titulo.Serie);

                if (dataReferencia.HasValue)
                {
                    dataReferencia = dataReferencia.Value.Date;
                }

                //if ((titulo.Situacao == "P") || (titulo.Situacao == "Q"))
                //{
                //    if (!dataReferencia.HasValue)
                //    {
                //        dataReferencia = titulo.DataVencimento;
                //    }

                //    //Trata dia util
                //    //Trata data referencia
                //    if (dataReferencia.HasValue)
                //    {
                //        if (titulo.DataVencimento < dataReferencia)
                //        {
                //            ClienteFornecedor clienteTitular = Contrato.Venda.Contrato.ListaVendaParticipante.Where(l => l.TipoParticipanteId == 1).FirstOrDefault().Cliente;
                //            bool achouProximoDiaUtil = false;
                //            DateTime dataUtil = DataVencimento.AddDays(-1);

                //            while (!achouProximoDiaUtil)
                //            {
                //                dataUtil = dataUtil.AddDays(1);
                //                Feriado feriado = null;
                //                if (clienteTitular.Correspondencia == "R")
                //                {
                //                    feriado = clienteTitular.EnderecoResidencial.UnidadeFederacao.ListaFeriado.Where(l => l.Data.Value.Date == dataUtil.Date).FirstOrDefault();
                //                }
                //                if (clienteTitular.Correspondencia == "C")
                //                {
                //                    feriado = clienteTitular.EnderecoComercial.UnidadeFederacao.ListaFeriado.Where(l => l.Data.Value.Date == dataUtil.Date).FirstOrDefault();
                //                }
                //                if (clienteTitular.Correspondencia == "O")
                //                {
                //                    feriado = clienteTitular.EnderecoOutro.UnidadeFederacao.ListaFeriado.Where(l => l.Data.Value.Date == dataUtil.Date).FirstOrDefault();
                //                }
                //                if (feriado == null)
                //                {
                //                    if ((dataUtil.DayOfWeek != DayOfWeek.Saturday) && (dataUtil.DayOfWeek != DayOfWeek.Sunday))
                //                    {
                //                        achouProximoDiaUtil = true;
                //                    }
                //                }
                //            }
                //            if (dataUtil >= dataReferencia.Value)
                //            {
                //                dataReferencia = DataVencimento;
                //            }
                //        }
                //    }
                //    //Trata data referencia

                //}




                listaTituloDetalheCredCob.Add(tituloDetalhe);
            }


            return listaTituloDetalheCredCob;
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
