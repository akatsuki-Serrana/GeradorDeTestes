﻿using GeradorDeTestes.Dominio.ModuloDisciplina;
using GeradorDeTestes.Dominio.ModuloMateria;
using GeradorDeTestes.Dominio.ModuloQuestao;
using GeradorDeTestes.Dominio.ModuloTeste;
using GeradorDeTestes.WinApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gerador.WinApp.ModuloTeste
{
    public partial class TelaTesteForm : Form
    {
        public List<Teste> testes;
        public List<Materia> materias;
        public List<Disciplina> disciplinas;
        public List<Questao> questoes;
        public List<Questao> questoesAleatorias;

        public TelaTesteForm(List<Materia> materias, 
            List<Disciplina> disciplinas, 
            List<Questao> questoes,
            List<Teste> testes)
        {
            InitializeComponent();

            this.ConfigurarDialog();

            this.materias = materias;
            this.disciplinas = disciplinas;
            this.questoes = questoes;
            this.testes = testes;

            CarregarComboBoxes(disciplinas, materias);
        }

        public Teste ObterTeste()
        {
            string titulo = txtTitulo.Text;

            Disciplina disciplina = (Disciplina)cmbDisciplina.SelectedItem;

            Materia materia = (Materia)cmbMateria.SelectedItem;

            bool ehRecuperacao = checkProva.Checked;

            int quantidadeDeQuestoes = (int)numericQtdQuestoes.Value;

            Teste teste = new Teste(titulo, ehRecuperacao, quantidadeDeQuestoes, disciplina, materia);

            teste.questoes = questoesAleatorias;

            return teste;
        }

        public void ConfigurarTela(Teste teste)
        {

        }

        public List<Questao> EmbaralharQuestoes(List<Questao> questoes, int numeroQuestoes)
        {
            Random random = new Random();

            for (int i = questoes.Count - 1; i > 1; i--)
            {
                int rnd = random.Next(i + 1);

                Questao questao = questoes[rnd];
                questoes[rnd] = questoes[i];
                questoes[i] = questao;
            }

            List<Questao> questoesAleatorias = new List<Questao>();
            for (int i = 0; i < numeroQuestoes; i++)
            {
                questoesAleatorias.Add(questoes[i]);
            }
            return questoesAleatorias;
        }

        private void CarregarComboBoxes(List<Disciplina> disciplinas, List<Materia> materias)
        {
            foreach (Disciplina disciplina in disciplinas)
            {
                cmbDisciplina.Items.Add(disciplina);
            }

            if (cmbDisciplina.Items.Count > 0)
                cmbDisciplina.SelectedIndex = 0;

            foreach (Materia materia in materias)
            {
                cmbMateria.Items.Add(materia);
            }
            if (cmbMateria.Items.Count > 0)
                cmbMateria.SelectedIndex = 0;
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            Teste teste = ObterTeste();

            string[] erros = teste.Validar();

            if (erros.Length > 0)
            {
                TelaPrincipalForm.Instancia.AtualizarRodape(erros[0]);

                DialogResult = DialogResult.None;
            }

            int numero = testes.FindAll(c => c.titulo == txtTitulo.Text && c.id != teste.id).Count();


            if (numero > 0)
            {
                TelaPrincipalForm.Instancia.AtualizarRodape("Título de 'Teste' já existente");

                DialogResult = DialogResult.None;
            }
        }
        private void btnSortear_Click(object sender, EventArgs e)
        {

            Materia materia = (Materia)cmbMateria.SelectedItem;
            this.questoesAleatorias = EmbaralharQuestoes(
                questoes.FindAll(q => materia.id == q.materia.id), (int)numericQtdQuestoes.Value);
            listboxQuestoes.Items.Clear();
            foreach (Questao questao in questoes)
            {
                listboxQuestoes.Items.Add(questao);
            }
        }
    }
}
