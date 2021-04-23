using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldDragon
{
    namespace Itens
    {
        class Iventario
        {

            public List<ItemIventario> Itens = new List<ItemIventario>();
            public ItemIventario this[int i]
            {
                get => Itens[i];
                set => Itens[i] = value;
            } 
            public long PesoTotal => Itens.Sum(x => x.item.Peso);
        }
        class ItemIventario
        {
            public Item item;
            public static implicit operator ItemIventario(Item i) => new ItemIventario(i);
            public ItemIventario(Item item) => this.item = item;

        }
        class ItemIventarioEstacado : ItemIventario
        {
            public uint qt;
            public ItemIventarioEstacado(uint qt, Item item) : base(item) => this.qt = qt;
        }
        class Item
        {
            public string Nome;
            public uint Peso;
            public uint? Preco;
            public Item(string nome, uint peso, uint? preco = null)
            {
                Nome = nome;
                Peso = peso;
                Preco = preco;
            }
        }
        enum TamanhoArma { P, M, G }
        class Arma : Item
        {
            public Rolagem Dano;
            public TamanhoArma Tamanho;
            public string Alcance;
            public string Especial;
            public string Tipo;
            public (uint Min, uint Multiplicador) Critico = (20, 2);
            public Arma(string nome, uint peso, Rolagem dano, TamanhoArma tamanho, string tipo, string alcance = "", string especial = "", uint? preco = null) : base(nome, peso, preco)
            {
                Dano = dano;
                Tamanho = tamanho;
                Tipo = tipo;
                Alcance = alcance;
                Especial = especial;
            }
        }
        class Protecao : Item
        {
            public bool Usando { get; set; }
            public int BonusCa;
            public uint? BonuxMaxDes;
            public int ReducaoMovimento;
            public Protecao(string nome, uint peso, int bonusCa, uint? bonuxMaxDes = null, int reducaoMovimento = 0, bool usando = true, uint? preco = null) : base(nome, peso, preco)
            {
                Usando = usando;
                BonusCa = bonusCa;
                BonuxMaxDes = bonuxMaxDes;
                ReducaoMovimento = reducaoMovimento;
            }
        }
    }

}
