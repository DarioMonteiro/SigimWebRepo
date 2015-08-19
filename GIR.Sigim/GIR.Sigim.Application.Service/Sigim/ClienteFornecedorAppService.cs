using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;
using GIR.Sigim.Application.Enums;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class ClienteFornecedorAppService : BaseAppService, IClienteFornecedorAppService
    {
        private IClienteFornecedorRepository clienteFornecedorRepository;

        public ClienteFornecedorAppService(IClienteFornecedorRepository clienteFornecedorRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.clienteFornecedorRepository = clienteFornecedorRepository;
        }

        public List<ClienteFornecedorDTO> ListarAtivos()
        {
            return clienteFornecedorRepository.ListarAtivos().To<List<ClienteFornecedorDTO>>();
        }

        public List<ClienteFornecedorDTO> ListarAtivosDeContrato()
        {
            return clienteFornecedorRepository.ListarAtivosDeContrato().To<List<ClienteFornecedorDTO>>();
        }

        public ClienteFornecedorDTO ObterPeloId(int? id)
        {
            return clienteFornecedorRepository.ObterPeloId(id).To<ClienteFornecedorDTO>();
        }

        //public List<ClienteFornecedorDTO> ListarClienteFornecedor(ClassificacaoClienteFornecedor classificacaoClienteFornecedor, SituacaoClienteFornecedor situacaoClienteFornecedor, TipoPessoa tipoPessoa)
        //{
            
        //    return clienteFornecedorRepository.ListarClienteFornecedor(classificacaoClienteFornecedor, situacaoClienteFornecedor, tipoPessoa).To<List<ClienteFornecedorDTO>>();  
        //}

        public List<ClienteFornecedorDTO> ListarClienteContratoAtivosPorNome(string nome)
        {
            var specification = (Specification<ClienteFornecedor>)new TrueSpecification<ClienteFornecedor>();
            specification &= ClienteFornecedorSpecification.NomeContem(nome);
            specification &= ClienteFornecedorSpecification.EhAtivo();
            specification &= ClienteFornecedorSpecification.EhClienteContrato();

            return clienteFornecedorRepository.ListarPeloFiltro(specification).To<List<ClienteFornecedorDTO>>();
        }

        public List<ClienteFornecedorDTO> ListarClienteOrdemCompraAtivosPorNome(string nome)
        {
            var specification = (Specification<ClienteFornecedor>)new TrueSpecification<ClienteFornecedor>();
            specification &= ClienteFornecedorSpecification.NomeContem(nome);
            specification &= ClienteFornecedorSpecification.EhAtivo();
            specification &= ClienteFornecedorSpecification.EhClienteOrdemCompra();

            return clienteFornecedorRepository.ListarPeloFiltro(specification).To<List<ClienteFornecedorDTO>>();
        }


        public List<ClienteFornecedorDTO> PesquisarClientesDeContratoAtivosPeloFiltro(ClienteFornecedorPesquisaFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<ClienteFornecedor>)new TrueSpecification<ClienteFornecedor>();
            specification &= ClienteFornecedorSpecification.EhAtivo();
            specification &= ClienteFornecedorSpecification.EhClienteContrato();

            bool EhTipoSelecaoContem = filtro.TipoSelecao == TipoPesquisa.Contem;
            switch (filtro.Campo)
            {
                case "rg":
                    specification &= EhTipoSelecaoContem ? ClienteFornecedorSpecification.RgContem(filtro.TextoInicio)
                        : ClienteFornecedorSpecification.RgNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "razaoSocial":
                    specification &= EhTipoSelecaoContem ? ClienteFornecedorSpecification.RazaoSocialContem(filtro.TextoInicio)
                        : ClienteFornecedorSpecification.RazaoSocialNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "cnpj":
                    specification &= EhTipoSelecaoContem ? ClienteFornecedorSpecification.CnpjContem(filtro.TextoInicio)
                        : ClienteFornecedorSpecification.CnpjNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "cpf":
                    specification &= EhTipoSelecaoContem ? ClienteFornecedorSpecification.CpfContem(filtro.TextoInicio)
                        : ClienteFornecedorSpecification.CpfNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "nomeFantasia":
                default:
                    specification &= EhTipoSelecaoContem ? ClienteFornecedorSpecification.NomeContem(filtro.TextoInicio)
                        : ClienteFornecedorSpecification.NomeNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
            }

            return clienteFornecedorRepository.Pesquisar(specification,
                                                         filtro.PageIndex,
                                                         filtro.PageSize,
                                                         filtro.OrderBy,
                                                         filtro.Ascending,
                                                         out totalRegistros,
                                                         l => l.PessoaFisica,
                                                         l => l.PessoaJuridica).To<List<ClienteFornecedorDTO>>();
        }

        public List<ClienteFornecedorDTO> PesquisarClientesDeOrdemCompraAtivosPeloFiltro(ClienteFornecedorPesquisaFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<ClienteFornecedor>)new TrueSpecification<ClienteFornecedor>();
            specification &= ClienteFornecedorSpecification.EhAtivo();
            specification &= ClienteFornecedorSpecification.EhClienteOrdemCompra();

            bool EhTipoSelecaoContem = filtro.TipoSelecao == TipoPesquisa.Contem;
            switch (filtro.Campo)
            {
                case "rg":
                    specification &= EhTipoSelecaoContem ? ClienteFornecedorSpecification.RgContem(filtro.TextoInicio)
                        : ClienteFornecedorSpecification.RgNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "razaoSocial":
                    specification &= EhTipoSelecaoContem ? ClienteFornecedorSpecification.RazaoSocialContem(filtro.TextoInicio)
                        : ClienteFornecedorSpecification.RazaoSocialNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "cnpj":
                    specification &= EhTipoSelecaoContem ? ClienteFornecedorSpecification.CnpjContem(filtro.TextoInicio)
                        : ClienteFornecedorSpecification.CnpjNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "cpf":
                    specification &= EhTipoSelecaoContem ? ClienteFornecedorSpecification.CpfContem(filtro.TextoInicio)
                        : ClienteFornecedorSpecification.CpfNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "nomeFantasia":
                default:
                    specification &= EhTipoSelecaoContem ? ClienteFornecedorSpecification.NomeContem(filtro.TextoInicio)
                        : ClienteFornecedorSpecification.NomeNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
            }

            return clienteFornecedorRepository.Pesquisar(specification,
                                                         filtro.PageIndex,
                                                         filtro.PageSize,
                                                         filtro.OrderBy,
                                                         filtro.Ascending,
                                                         out totalRegistros,
                                                         l => l.PessoaFisica,
                                                         l => l.PessoaJuridica).To<List<ClienteFornecedorDTO>>();
        }

    }
}