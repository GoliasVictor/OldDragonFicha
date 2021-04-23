
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using OldDragon;
using OldDragon.Atributo;
using OldDragon.Classes;
using OldDragon.Itens;
using static OldDragon.Dado;
namespace OldDragobTeste
{
    [TestClass]
    public class UnitTest1
    { 
        [TestMethod] 
        public void Personagem()
        {
            var personagem = new Personagem("dougras", Raca.Racas["ElfoNegro"], Classe.Mago, 30000, atributos: new Atributos(10, 14, 11, 16, 9, 7));
            personagem.Iventario.Itens.AddRange(new List<Item>() {
                new Protecao("Armadura de placas", 13, 7, 3, -2, true, 300),
                new Protecao("Escudo madeira", 2, 1, Preco: 3),
                new Arma("Adaga", 1, D4, TamanhoArma.G, "CO/PE"),
                new Arma("Espada", 1, D8, TamanhoArma.M, "CO"),
                new Arma("Flecha", 0.1, D6, TamanhoArma.P, "PE",Quantidade:20)
            });
            _ = personagem.JP;
        }
        [TestMethod]
        public void Classes()
        {
            var testClase = new ClasseUsuariaMagia("Jubiscreuson",
                                                   Classe.Tabelas.HomemDeArmas,
                                                   Classe.Tabelas.MagiasMago,
                                                   ("OlhaAssauto", Classe.Tabelas.TaletosLadrao),
                                                   ("SaiSatanas" , Classe.Tabelas.AfastarDadosvidaMortoVivo));
        }
    }
}
