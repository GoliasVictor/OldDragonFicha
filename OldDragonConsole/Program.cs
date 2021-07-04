using System;
using System.Collections.Generic;
using OldDragon;
using OldDragon.Atributo;
using OldDragon.Itens;
using static OldDragon.Dado;

namespace OldDragonConsole
{
	class Program
	{
		static Inventario MassaTestInventario = new Inventario() {
					new Protecao("Armadura de placas", 13, 7, 3, -2, true, new Dinheiro(300, Moedas.PO)),
					new Protecao("Escudo madeira"    ,  2, 1,       Preco: new Dinheiro(3  , Moedas.PO)),
					new Arma("Adaga", 1   , D4, TamanhoArma.G, new HashSet<TipoArma>(){ TiposArma.Co, TiposArma.Pe }),
					new Arma("Espada", 1  , D8, TamanhoArma.M, TiposArma.Co),
					new Arma("Flecha", 0.1, D6, TamanhoArma.P, TiposArma.Pe ,Quantidade:20)
				}; 
		static void Main(string[] args)
		{  
			var CriadorDePersonagem = new CriadorDePersonagem();
			CriadorDePersonagem.DefinirNome("Dougras")
								.DefinirRaca(Raca.Racas["ElfoNegro"])
								.DefinirClasse(Classes.Mago)
								.DefinirExperiencia(3000)
								.DefinirAlinhamento(Alinhamento.Caotico)
								.DefinirAtributos(new Atributos(10, 14, 11, 16, 9, 7))
								.AdicionarItens(MassaTestInventario);

			var personagem =  CriadorDePersonagem.FinalizarPersonagem();
			foreach( Item item in personagem.Inventario)
			{
				string Saida = "";
				if (item.Quantidade != 1) Saida += $"{item.Quantidade}x ";
				Saida += item.Nome;
				if (item.peso != 0      ) Saida += $" - {item.peso}Kg";
				if (item.Preco is object) Saida += $" - {item.Preco}";
				if (item is Protecao protecao) Saida += $" - {protecao.BonusCa}";
				if (item is Arma arma) Saida += $" - {arma.Dano}";
				Console.WriteLine(Saida);
			}
		}
	}
}
