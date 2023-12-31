﻿using GeradorDeTestes.Dominio.ModuloMateria;

namespace GeradorDeTestes.Dominio.ModuloQuestao
{
    [Serializable]
    public class Questao : EntidadeBase<Questao>
    {
        public string enunciado;
        public List<Alternativa> alternativas;
        public Alternativa resposta;
        public Materia materia;

        public Questao()
        {
            
        }

        public Questao(string enunciado, Alternativa resposta, List<Alternativa> alternativas, Materia materia)
        {
            this.enunciado = enunciado;
            this.resposta = resposta;
            this.alternativas = alternativas;
            this.materia = materia;
        }

        public Questao(int id, string enunciado, Alternativa resposta, List<Alternativa> alternativas, Materia materia)
        {
            this.id = id;
            this.enunciado = enunciado;
            this.resposta = resposta;
            this.alternativas = alternativas;
            this.materia = materia;
        }

        public override void AtualizarInformacoes(Questao registroAtualizado)
        {
            this.id = registroAtualizado.id;
            this.enunciado = registroAtualizado.enunciado;
            this.alternativas = registroAtualizado.alternativas;
            this.resposta = registroAtualizado.resposta;
            this.materia = registroAtualizado.materia;
        }

        public override string[] Validar()
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(enunciado))
                erros.Add("O campo 'Enunciado' é obrigatório");

            if (enunciado.Length < 5 )
                erros.Add("O campo 'Enunciado' deve conter no mínimo 5 caracteres");

            if (resposta == null)           
                erros.Add("A questão deve haver resposta");          

            if (alternativas.Count < 2)
                erros.Add("O campo 'Alternativas' precisa de ao menos 2 alternativas");

            if (alternativas.Count > 4)
                erros.Add("O campo 'Alternativas' pode ter ao máximo 4 alternativas");

            bool alternativasUnicas = true;
            for(int i = 0; i < alternativas.Count && alternativasUnicas; i++)
            {
                for(int j = i + 1; j < alternativas.Count; j++)
                {
                    if (alternativas[i].descricao == alternativas[j].descricao)
                    {
                        erros.Add("O campo 'Alternativas' não pode ter alternativa repetida");
                        alternativasUnicas = false;
                        break;
                    }
                }
            }

            return erros.ToArray();
        }

        public override string ToString()
        {
            return $"{enunciado}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Questao questao &&
                   id == questao.id &&
                   enunciado == questao.enunciado &&
                   EqualityComparer<List<Alternativa>>.Default.Equals(alternativas, questao.alternativas) &&
                   EqualityComparer<Alternativa>.Default.Equals(resposta, questao.resposta) &&
                   EqualityComparer<Materia>.Default.Equals(materia, questao.materia);
        }
    }
}
