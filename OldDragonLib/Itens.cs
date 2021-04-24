using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OldDragon;
namespace OldDragon
{
    namespace Itens
    {
        public class Iventario : Collection<Item>
        {
             
            public IEnumerable<Arma> Armas => from Item in this where Item is Arma select (Arma)Item;
            public double PesoTotal => this.Sum(x => x.Peso);
            public Iventario() : base() { }
            public Iventario(IEnumerable<Item> itens) : base() => AddRange(itens);
            public void AddRange(IEnumerable <Item> itens)
            {
                foreach (Item item in itens)
                    Add(item);
            }
        }

        public struct Dinheiro
        {
            public decimal Valor;
            public Moeda Moeda;
            public Dinheiro(decimal valor, Moeda moeda)
            {
                Valor = valor;
                Moeda = moeda ?? throw new ArgumentNullException(nameof(moeda));
            }
            public override string ToString()
            {
                return $"{Valor}{Moeda.Sigla}";
            }
        }
        public class Moeda
        {
            public string Sigla;
            public string Nome;
            public string Descricao;

            public Moeda(string sigla, string nome = default, string descricao = default)
            {
                Sigla = sigla ?? throw new ArgumentNullException(nameof(sigla));
                Nome = nome;
                Descricao = descricao;

            }
        }
        public static class Moedas
        {
            public static readonly Moeda PPL = new Moeda("PPL", "Peça de Platina");
            public static readonly Moeda PE  = new Moeda("PE" , "Peça de Eléctrum");
            public static readonly Moeda PO  = new Moeda("PO" , "Peça de ouro");
            public static readonly Moeda PP  = new Moeda("PP" , "Peça de Prata");
            public static readonly Moeda PC  = new Moeda("PO" , "Peça de Cobre");
        }
        public class Item
        {
            public uint Quantidade;
            public string Nome;
            public double peso;
            public Dinheiro? Preco;

            public double Peso
            {
                get => peso;
                set
                {
                    if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), value, "");
                    peso = value;
                }
            }

            public Item(string nome, double peso, Dinheiro? preco = default, uint quantidade = 1)
            {
                Nome = nome;
                Peso = peso;
                Preco = preco;
                Quantidade = quantidade;
            }
        }
        public class Protecao : Item
        {
            public bool Usando;
            public int BonusCa;
            public uint? BonuxMaxDes;
            public int ReducaoMovimento;

            public Protecao(string Nome, double Peso, int BonusCa, uint? BonuxMaxDes = null, int ReducaoMovimento = 0,
                            bool Usando = true, Dinheiro? Preco = default, uint Quantidade = 1) : base(Nome, Peso, Preco, Quantidade)
            {
                this.Usando = Usando;
                this.BonusCa = BonusCa;
                this.BonuxMaxDes = BonuxMaxDes;
                this.ReducaoMovimento = ReducaoMovimento;
            }
        }
        public enum TamanhoArma { P, M, G }
        public enum TipoRecarga { AcaoLivre, AcaoDeMovimento };
        public class Arma : Item
        {
            public Rolagem Dano;
            public TamanhoArma Tamanho;
            public AlcanceArma? Alcance;
            public TipoRecarga? Recarga;
            public string Especial;
            public HashSet<TipoArma> Tipo;
            public (uint Min, uint Multiplicador) Critico = (20, 2);
            public Arma(string Nome, double Peso, Rolagem Dano, TamanhoArma Tamanho, HashSet<TipoArma> Tipo,
                        AlcanceArma? Alcance = default, TipoRecarga? Recarga = default, string Especial = default, Dinheiro? Preco = default, uint Quantidade = 1) : base(Nome, Peso, Preco, Quantidade)
            {
                this.Dano = Dano;
                this.Tamanho = Tamanho;
                this.Tipo = Tipo ?? throw new ArgumentNullException(nameof(Tipo));
                this.Recarga = Recarga;
                this.Alcance = Alcance;
                this.Especial = Especial;
            }
        }
        public struct AlcanceArma
        {
            public string Tipo;
            public uint? Minimo;
            public uint? Medio;
            public uint? Maximo;

            public AlcanceArma(string tipo, uint? minimo, uint? medio, uint? maximo)
            {
                Tipo = tipo ?? throw new ArgumentNullException(nameof(tipo));
                Minimo = minimo;
                Medio = medio;
                Maximo = maximo;
            }
        }
        public class TipoArma
        {
            public readonly string Sigla;
            public readonly string Descricao;
            internal TipoArma(string Sigla, string Descricao)
            {
                this.Sigla = Sigla;
                this.Descricao = Descricao;
                TiposArma.ConjuntoTiposArma.Add(this);
            }
            public static implicit operator HashSet<TipoArma>(TipoArma TipoArma) => new HashSet<TipoArma> { TipoArma };
 
        }
        public static class TiposArma
        {

            public static HashSet<TipoArma> ConjuntoTiposArma = new HashSet<TipoArma>();
            public static readonly TipoArma Pe = new TipoArma("Pe", "Perfuracao");
            public static readonly TipoArma Co = new TipoArma("Co", "Corte");
            public static readonly TipoArma Im = new TipoArma("Im", "Impacto");
            public static TipoArma ObterTipo(string Sigla)
            {
                if (Sigla is null)
                    throw new ArgumentNullException(nameof(Sigla));
                return ConjuntoTiposArma.First((T) => string.Equals(T.Sigla, Sigla, StringComparison.OrdinalIgnoreCase));
            }
        }


    }
}
