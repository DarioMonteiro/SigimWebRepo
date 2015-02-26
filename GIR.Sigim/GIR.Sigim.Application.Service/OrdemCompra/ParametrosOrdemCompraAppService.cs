using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.Service.Contrato;
using GIR.Sigim.Application.Service.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public class ParametrosOrdemCompraAppService : BaseAppService, IParametrosOrdemCompraAppService
    {
        private IParametrosOrdemCompraRepository parametrosRepository;
        private IParametrosOrcamentoAppService parametrosOrcamentoAppService;
        private IParametrosContratoAppService parametrosContratoAppService;

        public ParametrosOrdemCompraAppService(
            IParametrosOrdemCompraRepository parametrosRepository,
            IParametrosOrcamentoAppService parametrosOrcamentoAppService,
            IParametrosContratoAppService parametrosContratoAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.parametrosRepository = parametrosRepository;
            this.parametrosOrcamentoAppService = parametrosOrcamentoAppService;
            this.parametrosContratoAppService = parametrosContratoAppService;
        }

        public ParametrosOrdemCompraDTO Obter()
        {
            return parametrosRepository.ListarTodos().FirstOrDefault().To<ParametrosOrdemCompraDTO>();
        }

        public void Salvar(ParametrosOrdemCompraDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            var parametros = dto.To<ParametrosOrdemCompra>();
            var entidade = this.Obter();

            if (entidade != null)
            {
                parametros.Id = entidade.Id;
                if ((dto.IconeRelatorio == null) && (!dto.RemoverImagem) && (entidade.IconeRelatorio.Length > 0))
                    parametros.IconeRelatorio = entidade.IconeRelatorio;
            }

            if (EhValido(parametros))
            {
                if (parametros.Id.HasValue)
                    parametrosRepository.Alterar(parametros);
                else
                    parametrosRepository.Inserir(parametros);

                parametrosOrcamentoAppService.AtualizarMascaraClasseInsumo(parametros.MascaraClasseInsumo);

                parametrosContratoAppService.AtualizarMascaraClasseInsumo(parametros.MascaraClasseInsumo);

                parametrosRepository.UnitOfWork.Commit();

                messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
            }
        }

        private bool EhValido(ParametrosOrdemCompra parametros)
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