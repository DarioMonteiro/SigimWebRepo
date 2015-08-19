using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.Admin;
using GIR.Sigim.Application.Filtros.Admin;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Domain.Entity.Admin;

namespace GIR.Sigim.Application.Service.Admin
{
    public class UsuarioFuncionalidadeAppService : BaseAppService, IUsuarioFuncionalidadeAppService
    {
        private IUsuarioFuncionalidadeRepository usuarioFuncionalidadeRepository;
        private IPerfilRepository perfilRepository;
        private IUsuarioPerfilRepository usuarioPerfilRepository;

        public UsuarioFuncionalidadeAppService(IUsuarioFuncionalidadeRepository usuarioFuncionalidadeRepository, 
                                               IPerfilRepository perfilRepository, 
                                               MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.usuarioFuncionalidadeRepository = usuarioFuncionalidadeRepository;
            this.perfilRepository = perfilRepository;
        }

        #region IUsuarioFuncionalidadeRepository Members

        public List<UsuarioFuncionalidadeDTO> ListarPeloUsuarioModulo(int UsuarioId, int ModuloId)
        {
            return usuarioFuncionalidadeRepository.ListarPeloFiltro(l => l.UsuarioId == UsuarioId && l.ModuloId == ModuloId, l => l.Usuario, l => l.Modulo).To<List<UsuarioFuncionalidadeDTO>>();
        }

        public List<UsuarioFuncionalidadeDTO> ListarPeloFiltro(UsuarioFuncionalidadeFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<UsuarioFuncionalidade>)new TrueSpecification<UsuarioFuncionalidade>();

            specification &= UsuarioFuncionalidadeSpecification.IgualAoUsuarioId(filtro.UsuarioId);
            specification &= UsuarioFuncionalidadeSpecification.IgualAoModuloId(filtro.ModuloId);

            return usuarioFuncionalidadeRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros, l => l.Usuario, l => l.Modulo).To<List<UsuarioFuncionalidadeDTO>>();
        }

        public bool Salvar(int UsuarioId, int ModuloId, int? PerfilId, List<UsuarioFuncionalidadeDTO> listaDto)
        {
            if (listaDto == null) throw new ArgumentNullException("dto");

            if (!ValidaSalvar(UsuarioId, ModuloId, PerfilId, listaDto)) { return false; }

            bool bolOK = true;


            //Salva as funcionalidades
            //=============================================================
            var usuarioFuncionalidade = new UsuarioFuncionalidade();
            var listaRemocao = ListarPeloUsuarioModulo(UsuarioId, ModuloId);

            foreach (var item in listaRemocao)
            {
                usuarioFuncionalidade = new UsuarioFuncionalidade();
                usuarioFuncionalidade = usuarioFuncionalidadeRepository.ObterPeloId(item.Id);
                usuarioFuncionalidadeRepository.Remover(usuarioFuncionalidade);
            }

            foreach (var item in listaDto)
            {
                usuarioFuncionalidade = new UsuarioFuncionalidade();
                usuarioFuncionalidade.Id = null;
                usuarioFuncionalidade.UsuarioId = UsuarioId;
                usuarioFuncionalidade.ModuloId = ModuloId;
                usuarioFuncionalidade.Funcionalidade = item.Funcionalidade;

                if (Validator.IsValid(usuarioFuncionalidade, out validationErrors))
                {
                    usuarioFuncionalidadeRepository.Inserir(usuarioFuncionalidade);
                    bolOK = true;
                }
                else
                {
                    bolOK = false;
                    break;
                }
            }

            //=============================================================

            //Falta salvar o perfil

            if (bolOK == true) 
            {
                usuarioFuncionalidadeRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
            }
            else
            {
                usuarioFuncionalidadeRepository.UnitOfWork.RollbackChanges();
                messageQueue.Add(Resource.Sigim.ErrorMessages.GravacaoErro, TypeMessage.Error);
            }

            return bolOK;
         
        }

        public bool Deletar(int UsuarioId, int ModuloId)
        {
            if (!ValidaDeletar(UsuarioId, ModuloId)) { return false; }

            bool bolOK = true;

            //Deleta as funcionalidades
            //=============================================================
            var usuarioFuncionalidade = new UsuarioFuncionalidade();
            var listaRemocao = ListarPeloUsuarioModulo(UsuarioId, ModuloId);

            foreach (var item in listaRemocao)
            {
                usuarioFuncionalidade = new UsuarioFuncionalidade();
                usuarioFuncionalidade = usuarioFuncionalidadeRepository.ObterPeloId(item.Id);
                try
                {
                    usuarioFuncionalidadeRepository.Remover(usuarioFuncionalidade);
                    bolOK = true;
                }
                catch (Exception)
                {
                    bolOK = false;
                    break;
                }
            }
            //=============================================================

            //Falta deletar o perfil - se houver

            if (bolOK == true)
            {
                usuarioFuncionalidadeRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
            }
            else
            {
                usuarioFuncionalidadeRepository.UnitOfWork.RollbackChanges();
                messageQueue.Add(Resource.Sigim.ErrorMessages.ExclusaoErro, TypeMessage.Error);
            }

            return bolOK;
        }

        //public List<TaxaAdministracaoDTO> ListarTodos()
        //{
        //    var lista = taxaAdministracaoRepository.ListarTodos(l => l.CentroCusto, l => l.Cliente).OrderBy(l => l.CentroCustoId).To<List<TaxaAdministracaoDTO>>();

        //    var vetTeste = new System.Collections.Hashtable();
        //    string texto;

        //    foreach (var item in Enumerable.Reverse(lista))
        //    {
        //        texto = item.CentroCusto.Codigo + "|" + item.ClienteId;

        //        if (vetTeste.ContainsKey(texto) == true) 
        //        {
        //            lista.Remove(item);
        //        }
        //        else
        //        {
        //            vetTeste.Add(texto, texto);
        //        }
        //    }

        //    return lista;

        //}

        #endregion


        #region Métodos Privados

        public bool ValidaSalvar(int UsuarioId, int ModuloId, int? PerfilId, List<UsuarioFuncionalidadeDTO> listaDto)
        {
            if (!ValidaUsuario(UsuarioId)) {return false;}

            if (!ValidaModulo(ModuloId)) {return false;}

            if (PerfilId != null)
            {
                var perfil = perfilRepository.ObterPeloId(PerfilId);
                if (perfil.ModuloId != ModuloId)
                {
                    messageQueue.Add(Application.Resource.Admin.ErrorMessages.PerfilNaoPertenceAoSistemaSelecionado, TypeMessage.Error);
                    return false;
                }
            }
           
            if (listaDto.Count == 0)
            {
                messageQueue.Add(Application.Resource.Admin.ErrorMessages.FuncionalidadesNaoMarcadas, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool ValidaDeletar(int UsuarioId, int ModuloId)
        {
            if (!ValidaUsuario(UsuarioId)) { return false; }

            if (!ValidaModulo(ModuloId)) { return false; }

            return true;
        }

        public bool ValidaUsuario(int UsuarioId)
        {
            if (UsuarioId == 0)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Usuário"), TypeMessage.Error);
                return false;
            }
            return true;
        }

        public bool ValidaModulo(int ModuloId)
        {
            if (ModuloId == 0)
            {
                messageQueue.Add(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Módulo"), TypeMessage.Error);
                return false;
            }
            return true;
        }

        #endregion


    }
}