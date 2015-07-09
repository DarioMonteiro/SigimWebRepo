using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class RateioAutomaticoAppService : BaseAppService, IRateioAutomaticoAppService
    {
        private IRateioAutomaticoRepository rateioAutomaticoRepository;

        public RateioAutomaticoAppService(IRateioAutomaticoRepository rateioAutomaticoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.rateioAutomaticoRepository = rateioAutomaticoRepository;
        }

        #region IRateioAutomaticoRepository Members

        public List<RateioAutomaticoDTO> ListarPeloTipoRateio(int TipoRateioId)
        {
            return rateioAutomaticoRepository.ListarPeloFiltro(l => l.TipoRateioId == TipoRateioId, l => l.TipoRateio, l => l.Classe, l => l.CentroCusto).To<List<RateioAutomaticoDTO>>();
        }

        public bool Salvar(int TipoRateioId, List<RateioAutomaticoDTO> listaDto)
        {
            if (listaDto == null) throw new ArgumentNullException("dto");

            if (ValidaSalvar(listaDto) == false) { return false; }

            var rateioAutomatico = new RateioAutomatico();
            var listaRemocao = ListarPeloTipoRateio(TipoRateioId);

            foreach (var item in listaRemocao)
            {
                rateioAutomatico = new RateioAutomatico();
                rateioAutomatico = rateioAutomaticoRepository.ObterPeloId(item.Id);
                rateioAutomaticoRepository.Remover(rateioAutomatico);
            }


            bool bolOK = true;
            foreach (var item in listaDto)
            {
                rateioAutomatico = new RateioAutomatico();
                rateioAutomatico.Id = null;
                rateioAutomatico.TipoRateioId = TipoRateioId;
                rateioAutomatico.CentroCustoId = item.CentroCusto.Codigo;
                rateioAutomatico.ClasseId = item.Classe.Codigo;
                rateioAutomatico.Percentual = item.Percentual;

                if (Validator.IsValid(rateioAutomatico, out validationErrors))
                {
                    rateioAutomaticoRepository.Inserir(rateioAutomatico);
                    bolOK = true;
                }
                else
                {
                    bolOK = false;
                    break;
                }
            }

            if (bolOK == true) 
            {
                rateioAutomaticoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
            }
            else
            {
                rateioAutomaticoRepository.UnitOfWork.RollbackChanges();
                messageQueue.Add("Erro na exclusão !", TypeMessage.Error);
            }

            return bolOK;
         
        }

        public bool Deletar(int TipoRateioId)
        {
            if (TipoRateioId == 0)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            bool bolOK = true;
            var rateioAutomatico = new RateioAutomatico();
            var listaRemocao = ListarPeloTipoRateio(TipoRateioId);

            foreach (var item in listaRemocao)
            {
                rateioAutomatico = new RateioAutomatico();
                rateioAutomatico = rateioAutomaticoRepository.ObterPeloId(item.Id);
                try
                {
                    rateioAutomaticoRepository.Remover(rateioAutomatico);
                    bolOK = true;
                }
                catch (Exception)
                {
                    bolOK = false;
                    break;
                }
            }

            if (bolOK == true)
            {
                rateioAutomaticoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
            }
            else
            {
                rateioAutomaticoRepository.UnitOfWork.RollbackChanges();
                messageQueue.Add("Erro na exclusão !", TypeMessage.Error);
            }

            return bolOK;
        }

        #endregion


        #region Métodos Privados

        public bool ValidaSalvar(List<RateioAutomaticoDTO> listaDto)
        {
            bool retorno = true;
            decimal decPercentualTotal = 0;

            foreach (var item in listaDto)
            {
                decPercentualTotal += item.Percentual;
            }

            if (decPercentualTotal > 100)
            {
                messageQueue.Add("O percentual total não pode ser maior que 100% !", TypeMessage.Error);
                retorno = false;
            }

            return retorno;
        }

        #endregion


    }
}