using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Resource.Sigim;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class ParametrosUsuarioFinanceiroAppService : BaseAppService , IParametrosUsuarioFinanceiroAppService
    {

        #region Properties

            private IParametrosUsuarioFinanceiroRepository parametrosUsuarioFinanceiroRepository;

        #endregion

        #region Constructor

            public ParametrosUsuarioFinanceiroAppService(IParametrosUsuarioFinanceiroRepository parametrosUsuarioFinanceiroRepository, ICentroCustoAppService centroCustoService, MessageQueue messageQueue)
                : base(messageQueue)
            {
                this.parametrosUsuarioFinanceiroRepository = parametrosUsuarioFinanceiroRepository;
            }

        #endregion

        #region Methods

        #endregion

        #region IParametrosUsuarioAppService Members

            public ParametrosUsuarioFinanceiroDTO ObterPeloIdUsuario(int? idUsuario)
            {
                return parametrosUsuarioFinanceiroRepository.ObterPeloId(idUsuario).To<ParametrosUsuarioFinanceiroDTO>();
            }

            public void Salvar(ParametrosUsuarioFinanceiroDTO dto)
            {
                if (dto == null)
                    throw new ArgumentNullException("dto");

                bool novoItem = false;

                var parametrosUsuarioFinanceiro = parametrosUsuarioFinanceiroRepository.ObterPeloId(dto.Id);
                if (parametrosUsuarioFinanceiro == null)
                {
                    parametrosUsuarioFinanceiro = new ParametrosUsuarioFinanceiro() { Id = dto.Id };
                    novoItem = true;
                }

                parametrosUsuarioFinanceiro.PortaSerial = dto.PortaSerial;

                parametrosUsuarioFinanceiro.TipoImpressora = GIR.Sigim.Application.DTO.Financeiro.TipoImpressoraEnum.Bematech.ToString().ToUpper();
                if (dto.TipoImpressoraEscolhida == GIR.Sigim.Application.DTO.Financeiro.TipoImpressoraEnum.Pertocheck)
                {
                    parametrosUsuarioFinanceiro.TipoImpressora = GIR.Sigim.Application.DTO.Financeiro.TipoImpressoraEnum.Pertocheck.ToString().ToUpper();
                }


                if (Validator.IsValid(parametrosUsuarioFinanceiro, out validationErrors))
                {
                    if (novoItem)
                        parametrosUsuarioFinanceiroRepository.Inserir(parametrosUsuarioFinanceiro);
                    else
                        parametrosUsuarioFinanceiroRepository.Alterar(parametrosUsuarioFinanceiro);

                    parametrosUsuarioFinanceiroRepository.UnitOfWork.Commit();

                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                }
                else
                    messageQueue.AddRange(validationErrors, TypeMessage.Error);
            }


        #endregion
    }
}
