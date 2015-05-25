using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class ParametrosFinanceiroAppService : BaseAppService, IParametrosFinanceiroAppService
    {
        private IParametrosFinanceiroRepository parametrosRepository;

        public ParametrosFinanceiroAppService(
            IParametrosFinanceiroRepository parametrosRepository,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.parametrosRepository = parametrosRepository;
        }

        public ParametrosFinanceiroDTO Obter()
        {
            return parametrosRepository.ListarTodos().FirstOrDefault().To<ParametrosFinanceiroDTO>();
        }

        public void Salvar(ParametrosFinanceiroDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            var parametros = dto.To<ParametrosFinanceiro>();

            if (dto.IconeRelatorio == null)
            {
                if (!dto.RemoverImagem)
                {
                    var entidade = Obter();
                    if (entidade != null)
                        parametros.IconeRelatorio = entidade.IconeRelatorio;
                }
            }

            if (EhValido(parametros))
            {
                if (parametros.Id.HasValue)
                    parametrosRepository.Alterar(parametros);
                else
                    parametrosRepository.Inserir(parametros);

                parametrosRepository.UnitOfWork.Commit();

                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
            }
        }

        private bool EhValido(ParametrosFinanceiro parametros)
        {
            return EhTamanhoDeArquivoValido(parametros.IconeRelatorio)
                && EhFormatoDeImagemValido(parametros.IconeRelatorio);
        }

        private bool EhTamanhoDeArquivoValido(byte[] imagem)
        {
            if (imagem != null && imagem.Length > 1 * 1024 * 1024)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.TamanhoArquivoSuperior1MB, TypeMessage.Error);
                return false;
            }

            return true;
        }

        private bool EhFormatoDeImagemValido(byte[] imagem)
        {
            try
            {
                if (imagem != null)
                {
                    ImageFormat[] formatosValidos = new[] { ImageFormat.Bmp, ImageFormat.Jpeg, ImageFormat.Gif, ImageFormat.Png };
                    MemoryStream ms = new MemoryStream(imagem);
                    using (var img = Image.FromStream(ms))
                    {
                        if (!formatosValidos.Contains(img.RawFormat))
                        {
                            messageQueue.Add(Resource.Sigim.ErrorMessages.FormatoImagemInvalido, TypeMessage.Error);
                            return false;
                        }
                    }
                }
            }
            catch
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.FormatoImagemInvalido, TypeMessage.Error);
                return false;
            }
            return true;
        }
    }
}
