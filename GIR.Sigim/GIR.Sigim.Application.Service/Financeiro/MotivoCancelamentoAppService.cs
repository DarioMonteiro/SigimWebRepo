﻿using System;
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

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class MotivoCancelamentoAppService : BaseAppService, IMotivoCancelamentoAppService
    {
        private IMotivoCancelamentoRepository motivoCancelamentoRepository;

        public MotivoCancelamentoAppService(IMotivoCancelamentoRepository motivoCancelamentoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.motivoCancelamentoRepository = motivoCancelamentoRepository;
        }

        #region IMotivoCancelamentoAppService Members

       
        public List<MotivoCancelamentoDTO> ListarPeloFiltro(MotivoCancelamentoFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<MotivoCancelamento>)new TrueSpecification<MotivoCancelamento>();


            return motivoCancelamentoRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<MotivoCancelamentoDTO>>();
        }

        public MotivoCancelamentoDTO ObterPeloId(int? id)
        {
            return motivoCancelamentoRepository.ObterPeloId(id).To<MotivoCancelamentoDTO>();
        }

        public bool Salvar(MotivoCancelamentoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            bool novoItem = false;

            var motivoCancelamento = motivoCancelamentoRepository.ObterPeloId(dto.Id);
            if (motivoCancelamento == null)
            {
                motivoCancelamento = new MotivoCancelamento();
                novoItem = true;
            }

            motivoCancelamento.Descricao = dto.Descricao;

            if (Validator.IsValid(motivoCancelamento, out validationErrors))
            {
                if (novoItem)
                    motivoCancelamentoRepository.Inserir(motivoCancelamento);
                else
                    motivoCancelamentoRepository.Alterar(motivoCancelamento);

                motivoCancelamentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                return true;
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public bool Deletar(int? id)
        {
            if (id == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                return false;
            }

            var motivoCancelamento = motivoCancelamentoRepository.ObterPeloId(id);

            try
            {
                motivoCancelamentoRepository.Remover(motivoCancelamento);
                motivoCancelamentoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.RegistroEmUso, motivoCancelamento.Descricao), TypeMessage.Error);
                return false;
            }
        }


        #endregion
      
    }
}