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
					new Arma("Flecha", 0.1, D6, TamanhoArma.P, TiposArma.Pe ,Quantidade:20),
					new Item("Colar de Ura Manji",0.1),
					new Item("Corda",5,new Dinheiro(1,Moedas.PO),2),
					new Arma("Arco Curto", 0.5, D0, TamanhoArma.P, TiposArma.Pe , new AlcanceArma("Arco",15,30,45),TipoRecarga.AcaoLivre,null,new Dinheiro(25,Moedas.PO))
            
				}; 
		static void Main(string[] args)
		{  
			var CriadorDePersonagem = new CriadorDePersonagem();
			CriadorDePersonagem.DefinirNome("Dougras")
								.DefinirRaca(Raca.Racas["ElfoNegro"])
								.DefinirClasse(Classes.HomemDeArmas)
								.DefinirExperiencia(30000)
								.DefinirAlinhamento(Alinhamento.Caotico)
								.DefinirAtributos(new Atributos(10, 14, 11, 16, 9, 7))
								.AdicionarItens(MassaTestInventario);

			var personagem =  CriadorDePersonagem.FinalizarPersonagem();
			string format = "║ {0,4} ║ {1,-21} ║ {2,5} ║ {3,5} ║ {4,2} ║ {5,4} ║\n";

			string Resultado = "";
			foreach( Item item in personagem.Inventario)
			{
				string[] StringItem = new string[7];
				StringItem[0] = $"{item.Quantidade}x ";
				StringItem[1] = item.Nome;
				
				if (item.peso != 0      ) StringItem[2] = $"{item.peso}Kg";
				if (item.Preco is object) StringItem[3] = $"{item.Preco}";
				if (item is Protecao protecao) StringItem[4] = $"+{protecao.BonusCa}";
				if (item is Arma arma) StringItem[5] = $"{arma.Dano}";
				Resultado += string.Format(format, StringItem);
			} 
			Console.WriteLine(personagem.CapacidadeCarga);
			Console.WriteLine(personagem.Nivel);
			Console.WriteLine(personagem.Atributos.For.Ajuste);
			Console.Write("╔══════════════════════════════════════════════════════════╗\n");
			Console.Write("║                        Iventario                         ║\n");
			Console.Write("╠══════╦═══════════════════════╦═══════╦═══════╦════╦══════╣\n");
			Console.Write("║  Qt  ║         Nome          ║  Peso ║ Preço ║ CA ║ Dano ║\n");
			Console.Write("╠══════╬═══════════════════════╬═══════╬═══════╬════╬══════╣\n");
			Console.Write(Resultado);
			Console.Write("╚══════╩═══════════════════════╩═══════╩═══════╩════╩══════╝\n"); 
		}
	}
}
