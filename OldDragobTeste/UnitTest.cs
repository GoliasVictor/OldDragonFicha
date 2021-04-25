
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Collections.Generic;
using OldDragon;
using OldDragon.Atributo; 
using OldDragon.Itens;
using static OldDragon.Dado;
namespace OldDragobTeste
{
    [TestClass]
    public class UnitTest1
    {
        Personagem personagem;
        [TestInitialize]
        public void Personagem()
        {
            personagem = new Personagem("dougras",
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
            _ = personagem.JP;
            Console.WriteLine(personagem.CapacidadeCarga);
        }
        [TestMethod]
        public void TestClasses()
        {
            var testClase = new ClasseUsuariaMagia("Jubiscreuson",
                                                   Tabelas.HomemDeArmas,
                                                   Tabelas.MagiasMago,
                                                   ("OlhaAssauto", Tabelas.TaletosLadrao),
                                                   ("SaiSatanas", Tabelas.AfastarDadosvidaMortoVivo));
        }
        [TestMethod]
        public void TestTipoArma()
        {
            try {
                TiposArma.ObterTipo("Capivara");
                Assert.Fail();
            }
            catch { }
            try
            {
                TiposArma.ObterTipo("CO");
            }
            catch
            {
                Assert.Fail();
            }
        }
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void TestIventario()
        {
            TestContext.WriteLine("test trace");
            Iventario iventario = new Iventario() {
                new Item("Colar de Ura Manji",0.1),
                new Item("Corda",5,new Dinheiro(1,Moedas.PO),2),
                new Protecao("Armadura de placas", 13, 7, 3, -2, true, new Dinheiro(300, Moedas.PO)),
                new Protecao("Escudo madeira"    ,  2, 1,       Preco: new Dinheiro(3  , Moedas.PO)),
                new Arma("Adaga", 1   , D4, TamanhoArma.G, new HashSet<TipoArma>(){ TiposArma.Co, TiposArma.Pe }),
                new Arma("Espada", 1  , D8, TamanhoArma.M, TiposArma.Co),
                new Arma("Flecha", 0.1, D6, TamanhoArma.P, TiposArma.Pe ,Quantidade:20),
                new Arma("Arco Curto", 0.5, null, TamanhoArma.P, TiposArma.Pe , new AlcanceArma("Arco",15,30,45),TipoRecarga.AcaoLivre,null,new Dinheiro(25,Moedas.PO))
            };
            Console.WriteLine("KDKRL");
            foreach( Item item in iventario)
            {
                string Saida = "";
                if (item.Quantidade != 1) Saida += $"{item.Quantidade}x ";
                Saida += item.Nome;
                if (item.peso != 0      ) Saida += $" - {item.peso}Kg";
                if (item.Preco is object) Saida += $" - {item.Preco}";
                if (item is Protecao    ) Saida += $" - {((Protecao)item).BonusCa}";
                if (item is Arma) Saida += $" - {((Arma)item).Dano}";
                TestContext.WriteLine(Saida);
            }
        }
         
    } 
}
