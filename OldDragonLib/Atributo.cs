using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using static OldDragon.Dado;

namespace OldDragon
{
    namespace Atributo
    {

        public struct Atributos
        {
            public readonly Forca For;
            public readonly Destreza Des;
            public readonly Constituicao Con;
            public readonly Inteligencia Int;
            public readonly Sabedoria Sab;
            public readonly Carisma Car;
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
                this.For = new Forca(For);
                this.Des = new Destreza(Des);
                this.Con = new Constituicao(Con);
                this.Int = new Inteligencia(Int);
                this.Sab = new Sabedoria(Sab);
                this.Car = new Carisma(Car);
            }
            public Atributos(Forca For, Constituicao Con, Destreza Des, Inteligencia Int, Sabedoria Sab, Carisma Car)
            {
                this.For = For ?? throw new ArgumentNullException(nameof(For));
                this.Con = Con ?? throw new ArgumentNullException(nameof(Con));
                this.Des = Des ?? throw new ArgumentNullException(nameof(Des));
                this.Int = Int ?? throw new ArgumentNullException(nameof(Int));
                this.Sab = Sab ?? throw new ArgumentNullException(nameof(Sab));
                this.Car = Car ?? throw new ArgumentNullException(nameof(Car));
            }
            public static Atributos operator +(Atributos Atr, ModAtributo Mod)
           => new Atributos(       (Forca)(Atr.For + Mod.For),
                            (Constituicao)(Atr.Con + Mod.Con),
                                (Destreza)(Atr.Des + Mod.Des),
                            (Inteligencia)(Atr.Int + Mod.Int),
                               (Sabedoria)(Atr.Sab + Mod.Sab),
                                 (Carisma)(Atr.Car + Mod.Car));
            public static Atributos operator -(Atributos Atr, ModAtributo Mod)
           => new Atributos(       (Forca)(Atr.For - Mod.For),
                            (Constituicao)(Atr.Con - Mod.Con),
                                (Destreza)(Atr.Des - Mod.Des),
                            (Inteligencia)(Atr.Int - Mod.Int),
                               (Sabedoria)(Atr.Sab - Mod.Sab),
                                 (Carisma)(Atr.Car - Mod.Car));

        }

        public struct ModAtributo
        {
            public int For;
            public int Des;
            public int Con;
            public int Int;
            public int Sab;
            public int Car;
            public ModAtributo(int For = 0, int Con = 0, int Des = 0, int Int = 0, int Sab = 0, int Car = 0)
            { 
                this.For = For;
                this.Des = Des;
                this.Con = Con;
                this.Int = Int;
                this.Sab = Sab;
                this.Car = Car; 
            }
        }

        public abstract class Atributo
        {
            public uint Valor { get; set; }
            public int Ajuste => (int)Math.Floor((((int)Valor) - 10) / 2.0);
            public Atributo(uint valor) => Valor = valor;
            public override string ToString() => Valor.ToString();
            public static implicit operator uint(Atributo d) => d.Valor;
            public static Atributo operator -(Atributo a, int b)
            { 
                Atributo c = (Atributo)a?.MemberwiseClone()?? throw new ArgumentNullException(nameof(a));
                c.Valor = (uint)(c.Valor - b);
                return c;
            }
            public static Atributo operator +(Atributo a, int b)
            {  
                Atributo c = (Atributo)a?.MemberwiseClone() ?? throw new ArgumentNullException(nameof(a));
                c.Valor = (uint)(c.Valor + b);
                return c;
            }
            public static Tabela<int> TabelaAtributos; 
            protected Dictionary<string,object> LinhaTabelaAtributo => TabelaAtributos[(int)Valor];
        }
        public class Forca : Atributo
        {
            public  (int CargaLeve, int CargaPesada, int CargaMaxima)CapacidadeCarga
            {
                get
                {
                    var CapacidadeCarga = Tabelas.CapacidadeCarga[Valor];
                    return((int)CapacidadeCarga["CargaMaxima"],
                           (int)CapacidadeCarga["CargaPesada"],
                           (int)CapacidadeCarga["CargaLeve"]);
                }
            }

            public static DataTable Tabela; 
            public Forca(uint valor) : base(valor) {}
        }

        public class Destreza : Atributo
        { 
            
            public int Arrombar => (int)LinhaTabelaAtributo["Arrombar"];
            public int Armadilhas => (int)LinhaTabelaAtributo["Armadilhas"];
            public int Furtividade => (int)LinhaTabelaAtributo["FurtividadEPungar"]; 
            public int Pungar => (int)LinhaTabelaAtributo["FurtividadEPungar"]; 
            public Destreza(uint valor) : base(valor) { }
        }
        public class Constituicao : Atributo
        {

            public int ChanceRessureicao => (int)LinhaTabelaAtributo["ChanceRessureicao"];
            public Constituicao(uint valor) : base(valor) { }
        }
        public class Inteligencia : Atributo
        {
            public int IdiomasAdicionais => (int)LinhaTabelaAtributo["IdiomasAdicionais"];
            public int ChanceAprenderMagia => (int)LinhaTabelaAtributo["ChanceAprenderMagia"];
            public Inteligencia(uint valor) : base(valor) { }
        }
        public class Sabedoria : Atributo
        {
            public (int C1, int C2, int C3) MagiasAdicionais => ((int,int,int))LinhaTabelaAtributo["MagiasAdicionais"];
            public Sabedoria(uint valor) : base(valor) { }
        }
        public class Carisma : Atributo
        {
            public int NumeroMaxSeguidores => (int)LinhaTabelaAtributo["NumeroSeguidores"];
            public int AjusteReacao => (int)LinhaTabelaAtributo["AjusteReacao"];
            public Rolagem QtMortoVivoAfastado => (Rolagem)LinhaTabelaAtributo["QtMortoVivoAfastado"];
            public Carisma(uint valor) : base(valor) { }
        }
    }
}
