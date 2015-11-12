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
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class ParametrosFinanceiroAppService : BaseAppService, IParametrosFinanceiroAppService
    {
        public const string delimitadorBloqueioSituacaoLiberado = "¨¨";

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
            ParametrosFinanceiroDTO parametros = parametrosRepository.ListarTodos(l => l.Cliente).FirstOrDefault().To<ParametrosFinanceiroDTO>();
            if (parametros.Cliente == null)
            {
                parametros.Cliente = new ClienteFornecedorDTO();
            }

            PreencherCheckBoxBloqueioSituacaoLiberado(parametros);

            return parametros;

        }

        public void Salvar(ParametrosFinanceiroDTO dto)
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.ParametroFinanceiroGravar))
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.PrivilegiosInsuficientes, TypeMessage.Error);
                return;
            }

            if (dto == null)
                throw new ArgumentNullException("dto");

            var parametros = dto.To<ParametrosFinanceiro>();
            var entidade = Obter();

            if (entidade != null)
            {
                parametros.Id = entidade.Id;
                if ((dto.IconeRelatorio == null) && (!dto.RemoverImagem) && (entidade.IconeRelatorio.Length > 0))
                    parametros.IconeRelatorio = entidade.IconeRelatorio;
            }

            parametros.BloqueioSituacaoLiberado = PreencherStringBloqueioSituacaoLiberado(dto);         

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

        public bool EhPermitidoSalvar()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.ParametroFinanceiroGravar))
                return false;

            return true;
        }


        private void PreencherCheckBoxBloqueioSituacaoLiberado(ParametrosFinanceiroDTO parametros)
        {
            string[] delimitador = new string[] { delimitadorBloqueioSituacaoLiberado };
            string[] bloqueioSituacaoLiberado = new string[]{};
            if (!string.IsNullOrEmpty(parametros.BloqueioSituacaoLiberado)){
                bloqueioSituacaoLiberado = parametros.BloqueioSituacaoLiberado.Split(delimitador, StringSplitOptions.None);
            }

            parametros.BloqueioCorrentista = false;
            parametros.BloqueioIdentificacao = false;
            parametros.BloqueioValorTitulo = false;
            parametros.BloqueioDataEmissao = false;
            parametros.BloqueioDataVencimento = false;
            parametros.BloqueioImpostos = false;
            parametros.BloqueioApropriacao = false;

            foreach (string bloqueio in bloqueioSituacaoLiberado)
            {
                if (bloqueio.ToUpper() == BloqueioSituacaoLiberado.Correntista.ObterDescricao().ToUpper())
                {
                    parametros.BloqueioCorrentista = true;
                    continue;
                }

                if (bloqueio.ToUpper() == BloqueioSituacaoLiberado.Identificacao.ObterDescricao().ToUpper())
                {
                    parametros.BloqueioIdentificacao = true;
                    continue;
                }

                if (bloqueio.ToUpper() == BloqueioSituacaoLiberado.ValorTitulo.ObterDescricao().ToUpper())
                {
                    parametros.BloqueioValorTitulo = true;
                    continue;
                }

                if (bloqueio.ToUpper() == BloqueioSituacaoLiberado.DataEmissao.ObterDescricao().ToUpper())
                {
                    parametros.BloqueioDataEmissao = true;
                    continue;
                }

                if (bloqueio.ToUpper() == BloqueioSituacaoLiberado.DataVencimento.ObterDescricao().ToUpper())
                {
                    parametros.BloqueioDataVencimento = true;
                    continue;
                }

                if (bloqueio.ToUpper() == BloqueioSituacaoLiberado.Apropriacao.ObterDescricao().ToUpper())
                {
                    parametros.BloqueioApropriacao = true;
                    continue;
                }

                if (bloqueio.ToUpper() == BloqueioSituacaoLiberado.Imposto.ObterDescricao().ToUpper())
                {
                    parametros.BloqueioImpostos = true;
                    continue;
                }

            }

        }

        private string PreencherStringBloqueioSituacaoLiberado(ParametrosFinanceiroDTO parametros)
        {
            string bloqueioSituacaoLiberado = "";

            if (parametros.BloqueioCorrentista)
            {
                bloqueioSituacaoLiberado = bloqueioSituacaoLiberado + BloqueioSituacaoLiberado.Correntista.ObterDescricao() + delimitadorBloqueioSituacaoLiberado;
            }

            if (parametros.BloqueioIdentificacao)
            {
                bloqueioSituacaoLiberado = bloqueioSituacaoLiberado + BloqueioSituacaoLiberado.Identificacao.ObterDescricao() + delimitadorBloqueioSituacaoLiberado;
            }

            if (parametros.BloqueioDataEmissao)
            {
                bloqueioSituacaoLiberado = bloqueioSituacaoLiberado + BloqueioSituacaoLiberado.DataEmissao.ObterDescricao() + delimitadorBloqueioSituacaoLiberado;
            }

            if (parametros.BloqueioDataVencimento)
            {
                bloqueioSituacaoLiberado = bloqueioSituacaoLiberado + BloqueioSituacaoLiberado.DataVencimento.ObterDescricao() + delimitadorBloqueioSituacaoLiberado;
            }

            if (parametros.BloqueioApropriacao)
            {
                bloqueioSituacaoLiberado = bloqueioSituacaoLiberado + BloqueioSituacaoLiberado.Apropriacao.ObterDescricao() + delimitadorBloqueioSituacaoLiberado;
            }

            if (parametros.BloqueioImpostos)
            {
                bloqueioSituacaoLiberado = bloqueioSituacaoLiberado + BloqueioSituacaoLiberado.Imposto.ObterDescricao() + delimitadorBloqueioSituacaoLiberado;
            }

            if (parametros.BloqueioValorTitulo)
            {
                bloqueioSituacaoLiberado = bloqueioSituacaoLiberado + BloqueioSituacaoLiberado.ValorTitulo.ObterDescricao() + delimitadorBloqueioSituacaoLiberado;
            }

            if (bloqueioSituacaoLiberado.Length > 0)
            {
                bloqueioSituacaoLiberado = bloqueioSituacaoLiberado.Substring(0, (bloqueioSituacaoLiberado.Length - 2));
            }

            return bloqueioSituacaoLiberado;

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
