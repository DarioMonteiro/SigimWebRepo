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

        public ClasseDTO ObterPeloCodigoEOrcamento(string codigo, int orcamentoId)
        {
            return ClasseRepository.ObterPeloCodigoEOrcamento(codigo, orcamentoId, l => l.ListaFilhos).To<ClasseDTO>();
        }

        public bool EhClasseValida(ClasseDTO Classe, int orcamentoId)
        {
            if (Classe == null)
            {
                if (orcamentoId > 0)
                    messageQueue.Add(Resource.Financeiro.ErrorMessages.ClasseNaoCadastradaNoOrcamento, TypeMessage.Error);
                else
                    messageQueue.Add(Resource.Financeiro.ErrorMessages.ClasseNaoCadastrada, TypeMessage.Error);

                return false;
            }
            return true;
        }

        public bool EhClasseUltimoNivelValida(ClasseDTO Classe, int orcamentoId)
        {
            if (!EhClasseValida(Classe, orcamentoId))
                return false;

            var filhosClasse = ClasseRepository.ListarPeloFiltro(l => l.Codigo.StartsWith(Classe.Codigo));
            if (filhosClasse.Count() > 1)
            {
                messageQueue.Add(Resource.Financeiro.ErrorMessages.ClasseUltimoNivel, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public List<TreeNodeDTO> ListarPeloOrcamento(int orcamentoId)
        {
            var arvore = ClasseRepository.ListarRaizes();
            if (orcamentoId > 0)
                return RemoverClassesNaoPertencentesAoOrcamento(arvore.ToList(), orcamentoId).To<List<TreeNodeDTO>>();
            else
                return arvore.To<List<TreeNodeDTO>>();;
        }

        #endregion

        private List<Classe> RemoverClassesNaoPertencentesAoOrcamento(List<Classe> arvore, int orcamentoId)
        {
            for (int i = arvore.Count - 1; i >= 0; i--)
            {
                if (arvore[i].ListaFilhos.Any())
                {
                    var filhos = RemoverClassesNaoPertencentesAoOrcamento(arvore[i].ListaFilhos.ToList(), orcamentoId);
                    if (filhos.Any())
                        arvore[i].ListaFilhos = RemoverClassesNaoPertencentesAoOrcamento(arvore[i].ListaFilhos.ToList(), orcamentoId);
                    else
                        arvore.Remove(arvore[i]);
                }
                else if (!arvore[i].ListaOrcamentoComposicao.Any(l => l.OrcamentoId == orcamentoId))
                    arvore.Remove(arvore[i]);
            }
            return arvore;
        }
    }
}