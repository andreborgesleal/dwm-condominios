using App_Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TimMaia.Models.Entidades;
using TimMaia.Models.Persistence;
using TimMaia.Models.Repositories;

namespace WcfParcParadisoService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ChamadoMobile : IChamadoMobile
    {
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "json/{id}")]
        public ParcChamado getChamadosAdministracao(string id)
        {
            ListViewChamadoAdministracao chamadoModel = new ListViewChamadoAdministracao();
            IList<OChamado> list = new List<OChamado>();
            using (ApplicationContext db = new ApplicationContext())
            {
                chamadoModel.db = db;
                IEnumerable<ChamadoViewModel> cham = chamadoModel.Bind(0, 10, null, null, DateTime.Today.AddDays(-20), DateTime.Today.AddDays(1), null, "", null, null).ToList();
                foreach (ChamadoViewModel c in cham)
                {
                    OChamado chamado = new OChamado()
                    {
                        nome_associado = c.nome_associado,
                        unidade = c.apto,
                        data_chamado = c.dt_ultima_interacao.Value.ToString("dd/MM/yyyy HH:mm") + "h.",
                        assunto = c.assunto,
                        chamadoId = c.chamadoId,
                        descricao_motivo = c.descricao_motivo,
                        descricao_status = c.descricao_status,
                    };

                    list.Add(chamado);
                }

            }

            ParcChamado parcChamado = new ParcChamado()
            {
                ochamado = list
            };

            return parcChamado;
        }

        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "atendimentos/{id}")]
        public OChamadoGlobal getAtendimentosByChamadoId(string id)
        {
            AtendimentoModel atendimentoModel = new AtendimentoModel();
            AtendimentoViewModel repository = new AtendimentoViewModel();
            Atendimento entity = new Atendimento();

            try
            {
                entity.chamadoId = int.Parse(id);
            }
            catch
            {
                return new OChamadoGlobal();
            }

            using (ApplicationContext db = new ApplicationContext())
            {
                atendimentoModel.db = db;
                repository = atendimentoModel.MapToRepository(entity);
            }

            IList<OAtendimento> list = new List<OAtendimento>();

            foreach(AtendimentoViewModel a in repository.atendimentos )
            {
                OAtendimento o = new OAtendimento()
                {
                    dt_atendimento = a.dt_atendimento.ToString("dd/MM/yyyy HH:mm") + "h.",
                    fluxo = a.fluxo,
                    mensagemResposta = a.mensagemResposta
                };
                list.Add(o);
            }

            OChamadoGlobal global = new OChamadoGlobal()
            {
                chamado = new OChamado()
                {
                    chamadoId = repository.chamadoId,
                    nome_associado = repository.chamado.nome_associado,
                    unidade = repository.chamado.apto,
                    assunto = repository.chamado.assunto,
                    descricao_motivo = repository.chamado.descricao_motivo,
                    descricao_status = repository.chamado.descricao_status,
                    data_chamado = repository.chamado.dt_chamado.ToString("dd/MM/yyyy HH:mm") + "h.",
                    mensagem_original = repository.chamado.mensagemOriginal
                },
                atendimentos = list
            };

            return global;
        }
    }

    public class OChamadoGlobal
    {
        public OChamado chamado { get; set; }
        public IEnumerable<OAtendimento> atendimentos { get; set; }
    }

    public class OAtendimento
    {
        public string dt_atendimento { get; set; }
        public string fluxo { get; set; }
        public string mensagemResposta { get; set; }
    }

    public class OChamado
    {
        public string nome_associado { get; set; }
        public string unidade { get; set; }
        public string data_chamado { get; set; }
        public string assunto { get; set; }
        public int chamadoId { get; set; }
        public string descricao_motivo { get; set; }
        public string descricao_status { get; set; }
        public string mensagem_original { get; set; }
    }

    public class ParcChamado
    {
        public IEnumerable<OChamado> ochamado { get; set; }
    }
}
