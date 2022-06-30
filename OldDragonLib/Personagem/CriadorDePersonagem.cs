using System;
using OldDragon.Atributo;
using System.Collections.Generic;
using OldDragon.Itens;

namespace OldDragon
{
	public class PersonagemNaoTerminadoException : Exception
	{
		public PersonagemNaoTerminadoException() { }
		public PersonagemNaoTerminadoException(string message) : base(message) { }
		public PersonagemNaoTerminadoException(string message, System.Exception inner) : base(message, inner) { }
		protected PersonagemNaoTerminadoException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
	public interface ICriadorDePersonagem
	{
		ICriadorDePersonagem AdicionarItens(IEnumerable<Item> itens);
		ICriadorDePersonagem DefinirAlinhamento(Alinhamento alinhamento);
		ICriadorDePersonagem DefinirAtributosBase(Atributos atributos);
		ICriadorDePersonagem DefinirClasse(Classe classe);
		ICriadorDePersonagem DefinirExperiencia(uint XP);
		ICriadorDePersonagem DefinirInventario(Inventario inventario);
		ICriadorDePersonagem DefinirNome(string nome);
		ICriadorDePersonagem DefinirRaca(Raca raca);
		Personagem FinalizarPersonagem();
		ICriadorDePersonagem Reiniciar();
	}
	public class CriadorDePersonagem : ICriadorDePersonagem
	{
		Personagem personagem;
		Atributos? atributosBase;
		int? DadosVida;
		Alinhamento? Alinhamento;
		public Personagem FinalizarPersonagem()
		{

			_ = personagem.Classe	?? throw new PersonagemNaoTerminadoException("Classe não definida");
			_ = personagem.Raca		?? throw new PersonagemNaoTerminadoException("Raça não definida");
			_ = Alinhamento			?? throw new PersonagemNaoTerminadoException("Alinhamento não definido");

			personagem.Alinhamento  = Alinhamento.Value;
			personagem.AtributosBase = atributosBase ?? Atributos.GerarAtributos();
			personagem.DadosVida = DadosVida ?? personagem.Classe.DadoVida(personagem.Nivel).Rolar();

			return personagem;
		}

		public CriadorDePersonagem()
		{
			Reiniciar();
		}
		public ICriadorDePersonagem Reiniciar()
		{
			personagem = new Personagem();
			return this;
		}
		public ICriadorDePersonagem DefinirInventario(Inventario inventario)
		{
			personagem.Inventario = inventario ?? throw new ArgumentNullException(nameof(inventario));
			return this;
		}
		public ICriadorDePersonagem AdicionarItens(IEnumerable<Item> itens)
		{
			personagem.Inventario.AddRange(itens);
			return this;
		}

		public ICriadorDePersonagem DefinirAlinhamento(Alinhamento alinhamento)
		{
			Alinhamento = alinhamento; 
			return this;
		}

		public ICriadorDePersonagem DefinirAtributosBase(Atributos atributos)
		{
			this.atributosBase = atributos;
			return this;
		}

		public ICriadorDePersonagem DefinirClasse(Classe classe)
		{
			personagem.Classe = classe ?? throw new ArgumentNullException(nameof(classe));
			return this;
		}
		public ICriadorDePersonagem DefinirExperiencia(uint XP)
		{
			personagem.XP = XP;
			return this;
		}
		public ICriadorDePersonagem DefinirNome(string nome)
		{
			if (string.IsNullOrWhiteSpace(nome))
				throw new ArgumentException($"'{nameof(nome)}' cannot be null or whitespace.", nameof(nome));

			personagem.Nome = nome;
			return this;
		}
		public ICriadorDePersonagem DefinirRaca(Raca raca)
		{
			personagem.Raca = raca ?? throw new ArgumentNullException(nameof(raca)); ;
			return this;
		}
	}
}