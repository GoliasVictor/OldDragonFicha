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
        static void Main(string[] args)
        {
            var personagem = new Personagem("dougras",
                                        Raca.Racas["ElfoNegro"],
                                        Classes.Mago,
                                        30000,
                                        Alinhamento.Caotico,
                                        atributos: new Atributos(10, 14, 11, 16, 9, 7),
                                        iventario: new Iventario() {
                new Protecao("Armadura de placas", 13, 7, 3, -2, true, new Dinheiro(300, Moedas.PO)),
                new Protecao("Escudo madeira"    ,  2, 1,       Preco: new Dinheiro(3  , Moedas.PO)),
                new Arma("Adaga", 1   , D4, TamanhoArma.G, new HashSet<TipoArma>(){ TiposArma.Co, TiposArma.Pe }),
                new Arma("Espada", 1  , D8, TamanhoArma.M, TiposArma.Co),
                new Arma("Flecha", 0.1, D6, TamanhoArma.P, TiposArma.Pe ,Quantidade:20)
            }); 
            foreach( Item item in personagem.Iventario)
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
