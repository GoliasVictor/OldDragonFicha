using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldDragon
{
    namespace Itens
    {
        public class Iventario
        {

            public List<Item> Itens = new List<Item>();
            public Item this[int i]
            {
                get => Itens[i];
                set => Itens[i] = value;
            }
            public IEnumerable<Arma> Armas => from Item in Itens where Item is Arma select (Arma)Item;
            public double PesoTotal => Itens.Sum(x => x.Peso);
        }
  
        public class Item
        {
            public uint Quantidade;
            public string Nome;
            private double peso;
            public uint? Preco;

            public double Peso
            {
                get => peso;
                set
                {
                    if (value < 0) throw new ArgumentOutOfRangeException("Peso", value, "");
                    peso = value;
                }
            }

            public Item(string nome, double peso, uint? preco = null, uint quantidade = 1)
            {
                Nome = nome;
                Peso = peso;
                Preco = preco;
                Quantidade = quantidade;
            }
        }
        public  enum TamanhoArma { P, M, G }
        public class Arma : Item
        {
            public Rolagem Dano;
            public TamanhoArma Tamanho;
            public string Alcance;
            public string Especial;
            public string Tipo;
            public (uint Min, uint Multiplicador) Critico = (20, 2);
            public Arma(string Nome, double Peso, Rolagem Dano, TamanhoArma Tamanho, string Tipo, string Alcance = "", string Especial = "", uint? Preco = null, uint Quantidade = 1) : base(Nome, Peso, Preco,Quantidade)
            {
                this.Dano = Dano;
                this.Tamanho = Tamanho;
                this.Tipo = Tipo;
                this.Alcance = Alcance;
                this.Especial = Especial;
            }
        }
        public class Protecao : Item
        {
            public bool Usando { get; set; }
            public int BonusCa;
            public uint? BonuxMaxDes;
            public int ReducaoMovimento;
            public Protecao(string Nome, double Peso, int BonusCa, uint? BonuxMaxDes = null, int ReducaoMovimento = 0, bool Usando = true, uint? Preco = null, uint Quantidade = 1) : base(Nome, Peso, Preco,Quantidade)
            {
                this.Usando = Usando;
                this.BonusCa = BonusCa;
                this.BonuxMaxDes = BonuxMaxDes;
                this.ReducaoMovimento = ReducaoMovimento;
            }
        }
    }
}
