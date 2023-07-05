﻿using GeradorDeTestes.Dominio.ModuloDisciplina;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTestes.Dominio.ModuloDisciplina
{
    public class Disciplina : EntidadeBase<Disciplina>
    {
        public string nome;
       

        public Disciplina()
        {
           

        }

        public Disciplina(string nome)
        {
            this.nome = nome;
           
        }

        public Disciplina(int id, string nome)
        {
            this.id = id;
            this.nome = nome;
           
        }

        public override void AtualizarInformacoes(Disciplina registroAtualizado)
        {
            this.id = registroAtualizado.id;
            this.nome = registroAtualizado.nome;
          
        }

        public override string[] Validar()
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(nome))
                erros.Add("O campo nome é obrigatório");



            if (nome.Count() < 4)
                erros.Add("O campo 'Valor' não pode receber o valor 0");

            return erros.ToArray();
        }

        public override string ToString()
        {
            return $"Id: {id} Descrição: {nome}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Disciplina Disciplina &&
                   id == Disciplina.id &&
                   nome == Disciplina.nome;
               
        }

    
  
    }
}