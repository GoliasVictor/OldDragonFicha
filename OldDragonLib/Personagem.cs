using System;
using OldDragon.Atributo;
using System.Collections.Generic;
using OldDragon.Itens;
using System.Linq;

namespace OldDragon
{
	public class Personagem
	{
		public string Nome;
		public Raca Raca;
		public uint XP;
		public int DadosVida;
		public Alinhamento Alinhamento;
		public Classe Classe;
		public Atributos AtributosBase;
		public Inventario Inventario = new Inventario();
		public List<int> BonusCAOutros = new List<int>();
		public uint Nivel => Classe.Nivel(XP);
		public Dado DV => Classe.DadoVida(Nivel).Dado;
		public int PV => DadosVida + Atributos.Con.Ajuste;
		public uint JP => Classe.JogadaProtecao(Nivel);
		public (uint MaoPrincipal, uint MaoSecundaria) BA => Classe.BaseAtaque(Nivel);
		public Atributos Atributos { 
			get => AtributosBase + Raca.ModAtributos; 
			set => AtributosBase = value - Raca.ModAtributos; 
		}
		public int CA
		{
			get
			{ 
				var CA = 10;
				int BonusDes = Atributos.Des.Ajuste;  
				foreach (Protecao protecao in Inventario.ItensProtecaoUsando)
				{
					CA += protecao.BonusCa;
					if (protecao.BonusMaxDes is object && BonusDes > protecao.BonusMaxDes)
						BonusDes = (int)protecao.BonusMaxDes;
				}
				return CA + BonusDes + BonusCAOutros.Sum();
			}
		}
		public uint Movimento
		{
			get
			{
				if (CapacidadeCarga == Carga.CargaMaxima)
					return 1; 
				return (uint)(Raca.Movimento + RedMovimento + Inventario.ItensProtecaoUsando.Sum((protecao) => protecao.ReducaoMovimento));
			}
		}
		public Carga CapacidadeCarga
		{
			get
			{
				var (CargaLeve, CargaPesada, CargaMaxima) = Atributos.For.CapacidadeCarga;
				var PesoTotal = Inventario.PesoTotal;
				if (PesoTotal >= CargaMaxima) return Carga.CargaMaxima;
				if (PesoTotal >= CargaPesada) return Carga.CargaPesada;
				if (PesoTotal >= CargaLeve	) return Carga.CargaLeve;
				else return Carga.SemCarga;
			}
		}
		public int? RedMovimento
		{
			get
			{
				switch (CapacidadeCarga)
				{
					case Carga.CargaLeve: return 0;
					case Carga.SemCarga: return -1;
					case Carga.CargaPesada: return -2;
					case Carga.CargaMaxima:
					default: return null;
				}
			}
		}
		internal Personagem() { }
	}

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
		ICriadorDePersonagem DefinirAtributos(Atributos atributos);
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
		Atributos? atributos;
		int? DadosVida;
		Alinhamento? Alinhamento;
		public Personagem FinalizarPersonagem()
		{

			_ = personagem.Classe	?? throw new PersonagemNaoTerminadoException("Classe não definida");
			_ = personagem.Raca		?? throw new PersonagemNaoTerminadoException("Raça não definida");
			_ = Alinhamento			?? throw new PersonagemNaoTerminadoException("Alinhamento não definido");

			personagem.Alinhamento  = Alinhamento.Value;
			personagem.Atributos = atributos ?? Atributos.GerarAtributos();
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

		public ICriadorDePersonagem DefinirAtributos(Atributos atributos)
		{
			this.atributos = atributos;
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