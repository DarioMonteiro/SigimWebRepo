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

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class ClasseAppService : BaseAppService, IClasseAppService
    {
        private IClasseRepository ClasseRepository;
        private IUsuarioRepository usuarioRepository;

        public ClasseAppService(IClasseRepository ClasseRepository, IUsuarioRepository usuarioRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.ClasseRepository = ClasseRepository;
            this.usuarioRepository = usuarioRepository;
        }

        #region IClasseService Members

        public ClasseDTO ObterPeloCodigo(string codigo)
        {
            return ClasseRepository.ObterPeloCodigo(codigo, l => l.ListaFilhos).To<ClasseDTO>();
        }

        public bool EhClasseValida(ClasseDTO Classe)
        {
            if (Classe == null)
            {
                messageQueue.Add(Resource.Financeiro.ErrorMessages.ClasseNaoCadastrada, TypeMessage.Error);
                return false;
            }
            return true;
        }

        public bool EhClasseUltimoNivelValida(ClasseDTO Classe)
        {
            if (!EhClasseValida(Classe))
                return false;

            var filhosClasse = ClasseRepository.ListarPeloFiltro(l => l.Codigo.StartsWith(Classe.Codigo));
            if (filhosClasse.Count() > 1)
            {
                messageQueue.Add(Resource.Financeiro.ErrorMessages.ClasseUltimoNivel, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public List<TreeNodeDTO> ListarRaizes()
        {
            return ClasseRepository.ListarRaizes().To<List<TreeNodeDTO>>();
        }

        #endregion
    }
}