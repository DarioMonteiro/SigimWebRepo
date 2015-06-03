using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class LogOperacaoAppService : BaseAppService, ILogOperacaoAppService
    {
        private ILogOperacaoRepository logOperacaoRepository;

        public LogOperacaoAppService(ILogOperacaoRepository logOperacaoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.logOperacaoRepository = logOperacaoRepository;
        }

        #region ILogOperacaoAppService Members

        public void Gravar(string descricao, string nomeRotina, string nomeTabela, string nomeComando, string dados)
        {
            try
            {
                LogOperacao log = new LogOperacao() {
                    Data = DateTime.Now,
                    LoginUsuario = UsuarioLogado.Login,
                    Descricao = descricao,
                    NomeRotina = nomeRotina,
                    NomeTabela = nomeTabela,
                    NomeComando = nomeComando,
                    Dados = dados,
                    HostName = UsuarioLogado.HostName
                };

                logOperacaoRepository.Inserir(log);
                logOperacaoRepository.UnitOfWork.Commit();
            }
            catch (Exception exception)
            {
                QueueExeptionMessages(exception);
            }
        }

        #endregion
    }
}