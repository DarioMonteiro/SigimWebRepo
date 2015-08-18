using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class MaterialClasseInsumoAppService : BaseAppService, IMaterialClasseInsumoAppService
    {

        private IMaterialClasseInsumoRepository materialClasseInsumoRepository;

        public MaterialClasseInsumoAppService(IMaterialClasseInsumoRepository materialClasseInsumoRepository, 
                                              MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.materialClasseInsumoRepository = materialClasseInsumoRepository;
        }

        public MaterialClasseInsumoDTO ObterPeloCodigo(string codigo)
        {
            return materialClasseInsumoRepository.ObterPeloCodigo(codigo, l => l.ListaFilhos).To<MaterialClasseInsumoDTO>();
        }

        public bool EhClasseInsumoValida(MaterialClasseInsumoDTO classeInsumo)
        {
            if (classeInsumo == null)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.ClasseInsumoNaoCadastrada, TypeMessage.Error);

                return false;
            }
            return true;
        }

        public bool EhClasseInsumoUltimoNivelValida(MaterialClasseInsumoDTO classeInsumo)
        {
            if (!EhClasseInsumoValida(classeInsumo))
                return false;

            var filhosClasse = materialClasseInsumoRepository.ListarPeloFiltro(l => l.Codigo.StartsWith(classeInsumo.Codigo));
            if (filhosClasse.Count() > 1)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.ClasseUltimoNivel, TypeMessage.Error);
                return false;
            }

            return true;
        }


        public List<TreeNodeDTO> ListarRaizes()
        {
            var arvore = materialClasseInsumoRepository.ListarRaizes();
            return arvore.To<List<TreeNodeDTO>>(); ;
        }
    }
}
