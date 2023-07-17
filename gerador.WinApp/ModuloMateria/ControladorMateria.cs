﻿using FluentResults;
using GeradorDeTestes.Dominio.ModuloDisciplina;
using GeradorDeTestes.Dominio.ModuloMateria;
using GeradorDeTestes.Dominio.ModuloQuestao;
using GerardorDeMaterias.Aplicacao.ModuloMateria;
using GerardorDeTestes.Aplicacao.ModuloQuestao;

namespace GeradorDeTestes.WinApp.ModuloMateria
{
    public class ControladorMateria : ControladorBase
    {
        private TabelaMateriaControl tabelaMateria;
        private readonly IRepositorioMateria repositorioMateria;
        private readonly IRepositorioDisciplina repositorioDisciplina;
        private readonly IRepositorioQuestao repositorioQuestao;
        private readonly ServicoMateria servicoMateria;

        public ControladorMateria(IRepositorioMateria repositorioMateria,
            IRepositorioDisciplina repositorioDisciplina,
            IRepositorioQuestao repositorioQuestao,
            ServicoMateria servicoMateria)
        {
            this.repositorioMateria = repositorioMateria;
            this.repositorioDisciplina = repositorioDisciplina;
            this.repositorioQuestao = repositorioQuestao;
            this.servicoMateria = servicoMateria;
        }

        public override string ToolTipInserir { get { return "Inserir nova Matéria"; } }

        public override string ToolTipEditar { get { return "Editar Matérias Existentes"; } }

        public override string ToolTipExcluir { get { return "Excluir Matérias Existentes"; } }

        public override void Inserir()
        {
            TelaMateriaForm telaMateria = new TelaMateriaForm(repositorioMateria.SelecionarTodos());
            telaMateria.CarregarDisciplinas(this.repositorioDisciplina.SelecionarTodos());

            telaMateria.onGravarRegistro = servicoMateria.Inserir;

            telaMateria.ShowDialog();

            CarregarMaterias();
        }
     
        public override void Editar()
        {
            Materia materia = ObterMateriaSelecionada();

            if(materia == null)
            {
                MessageBox.Show($"Selecione uma Matéria primeiro!",
                    "Edição de Matérias",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                return;
            }
            TelaMateriaForm telaMateria = new TelaMateriaForm(repositorioMateria.SelecionarTodos());
            telaMateria.CarregarDisciplinas(this.repositorioDisciplina.SelecionarTodos());
            telaMateria.ConfigurarTela(materia);

            telaMateria.onGravarRegistro = servicoMateria.Editar;

            telaMateria.ShowDialog();

            CarregarMaterias();
        }

        private Materia ObterMateriaSelecionada()
        {
            int id = tabelaMateria.ObterIdSelecionado();

            if (id == -1)
                return null;

            return repositorioMateria.SelecionarPorId(id);
        }

        public override void Excluir()
        {
            Materia materia = ObterMateriaSelecionada();

            if (materia == null)
            {
                MessageBox.Show($"Selecione uma Matéria primeiro!",
                    "Exclusão de Matérias",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                return;
            }

            if (repositorioQuestao.SelecionarTodos().Find(q => q.materia.id == materia.id) != null)
            {
                MessageBox.Show("A Matéria não pode ser Excluída pois esta em uma Questão", "Exclusão de Matérias",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                return;
            }
            DialogResult opcaoEscolhida = MessageBox.Show($"Deseja excluir a Matéria {materia.Nome}?", "Exclusão de Matérias",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (opcaoEscolhida == DialogResult.OK)
            {
                Result resultado = servicoMateria.Excluir(materia);
                if (resultado.IsFailed)
                {
                    MessageBox.Show($"{resultado.Errors[0].Message}",
                   "Exclusão de Materias",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }

            CarregarMaterias();
        }       

        public override UserControl ObterListagem()
        {
           if(tabelaMateria == null)
                tabelaMateria = new TabelaMateriaControl();

            CarregarMaterias();

           return tabelaMateria;           
        }
        private void CarregarMaterias()
        {
            List<Materia> materias = repositorioMateria.SelecionarTodos();

            tabelaMateria.AtualizarRegistros(materias);

            TelaPrincipalForm.Instancia.AtualizarRodape("Visualizando" + " " + materias.Count + " " + "Matérias");
        }

        public override string ObterTipoCadastro()
        {
            return "Cadastro de Matérias";
        }
    }
}
