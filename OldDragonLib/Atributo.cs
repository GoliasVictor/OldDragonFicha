using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using static OldDragon.Dado;
using static OldDragon.Tabelas;

namespace OldDragon
{
	namespace Atributo
	{

		enum TipoAtributo
		{
			 Forca,
			 Destreza,
			 Constituicao,
			 Inteligencia,
			 Sabedoria,
			 Carisma
		}

		public struct Atributos
		{
			public readonly Atributo For, Des, Con, Int, Sab, Car;
			public static Atributos GerarAtributos()
			{
				Rolagem rolagem = new Rolagem(D6, 3);
				return new Atributos((uint)rolagem.Rolar(),
									 (uint)rolagem.Rolar(),
									 (uint)rolagem.Rolar(),
									 (uint)rolagem.Rolar(),
									 (uint)rolagem.Rolar(),
									 (uint)rolagem.Rolar());
			}

			public Atributos(uint For, uint Con, uint Des, uint Int, uint Sab, uint Car)
			{
				this.For = new Atributo(For);
				this.Des = new Atributo(Des);
				this.Con = new Atributo(Con);
				this.Int = new Atributo(Int);
				this.Sab = new Atributo(Sab);
				this.Car = new Atributo(Car);
			}
		}

		public record struct ModAtributo(int For = 0, int Con = 0, int Des = 0, int Int = 0, int Sab = 0, int Car = 0)
		{
			public static Atributos operator +(Atributos Atr, ModAtributo Mod)
			  => new Atributos((uint)(Atr.For + Mod.For),
							   (uint)(Atr.Con + Mod.Con),
							   (uint)(Atr.Des + Mod.Des),
							   (uint)(Atr.Int + Mod.Int),
							   (uint)(Atr.Sab + Mod.Sab),
							   (uint)(Atr.Car + Mod.Car));
		}

		public class Atributo

		{
			public uint Valor { get; set; }
			public int Ajuste => (int)Math.Floor(Valor/2.0) - 5;
			public Atributo(uint valor) => Valor = valor;
			public static implicit operator uint(Atributo d) => d.Valor;
			public static implicit operator int(Atributo d) => (int)d.Valor;
			public override string ToString() => Valor.ToString();

		}
		
	}
}
