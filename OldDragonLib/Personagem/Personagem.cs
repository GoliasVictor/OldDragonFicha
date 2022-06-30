using System;
using OldDragon.Atributo;
using System.Collections.Generic;
using OldDragon.Itens;
using System.Linq;

namespace OldDragon
{
	public partial class Personagem
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
				var (CargaLeve, CargaPesada, CargaMaxima) = VlCapacidadeCarga;
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

	
}