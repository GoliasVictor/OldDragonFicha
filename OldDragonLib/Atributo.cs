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
                this.For = For;
                this.Con = Con;
                this.Des = Des;
                this.Int = Int;
                this.Sab = Sab;
                this.Car = Car;
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

            private uint valor;
            public uint Valor
            {
                get => valor;
                set
                {
                    if (value < 0) throw new ArgumentOutOfRangeException("Valor Atributo", value, "");
                    valor = value;
                }
            }
            public int Ajuste => (int)Math.Floor((((int)Valor) - 10) / 2.0);
            public Atributo(uint valor) => Valor = valor;
            public override string ToString() => Valor.ToString();
            public static implicit operator uint(Atributo d) => d.Valor;
            public static Atributo operator -(Atributo a, int b)
            {
                Atributo c = (Atributo)a.MemberwiseClone();
                c.Valor = (uint)(c.Valor - b);
                return c;
            }
            public static Atributo operator +(Atributo a, int b)
            {
                Atributo c = (Atributo)a.MemberwiseClone();
                c.Valor = (uint)(c.Valor + b);
                return c;
            }
            public static Tabela<int> TabelaAtributos;

            static Atributo()
            {
                string[] Colunas = 
                {
                    "Atributo",
                    "Ajuste",
                    "Armadilhas",
                    "Arrombar",
                    "FurtividadEPungar",
                    "ChanceRessureicao",
                    "IdiomasAdicionais",
                    "ChanceAprenderMagia",
                    "MagiasAdicionais",
                    "NumeroSeguidores",
                    "AjusteReacao",
                    "QtMortoVivoAfastado"
                };
                bool VerificaoLinha(int i, Dictionary<string,object> Linha)
                {
                    var Atributo =((int Min,int Max))Linha["Atributo"];
                    return Atributo.Min >= i || Atributo.Max <= i;
                }
                var tabela = new object[][] {
                    new object[]{( 0, 1), -5, -25, -25, -25,  0, 0, 0,(0,0,0), 0,-25, new Rolagem(0, 0)},
                    new object[]{( 2, 3), -4, -20, -20, -20,  0, 0, 0,(0,0,0), 0,-20, new Rolagem(0, 0)},
                    new object[]{( 4, 5), -3, -15, -15, -15,  0, 0, 0,(0,0,0), 0,-15, new Rolagem(0, 0)},
                    new object[]{( 6, 7), -2, -10, -10, -10,  1, 0, 0,(0,0,0), 0,-10, new Rolagem(0, 0)},
                    new object[]{( 8, 9), -1,  -5,  -5,  -5,  5, 1, 5,(0,0,0), 0, -5, new Rolagem(1   )},
                    new object[]{(10,11),  0,   0,   0,   0, 10, 1,10,(0,0,0), 1,  0, new Rolagem(D2  )},
                    new object[]{(12,13), +1,   0,  +5,   0, 25, 1,20,(0,0,0), 2, +5, new Rolagem(D3  )},
                    new object[]{(14,15), +2,   0, +10,  +5, 50, 1,25,(0,0,0), 3,+10, new Rolagem(D4  )},
                    new object[]{(16,17), +3,  +5, +15, +10, 75, 2,35,(1,0,0), 4,+15, new Rolagem(D6  )},
                    new object[]{(18,19), +4, +10, +20, +15, 95, 3,45,(2,0,0), 5,+20, new Rolagem(D8  )},
                    new object[]{(20,21), +5, +15, +25, +20,100, 4,55,(2,1,0), 6,+25, new Rolagem(D4,2)},
                    new object[]{(22,23), +6, +20, +30, +25,100, 5,65,(2,2,0), 7,+30, new Rolagem(D10 )},
                    new object[]{(24,25), +7, +25, +35, +30,100, 6,75,(2,2,1), 8,+35, new Rolagem(D12 )},
                    new object[]{(26,27), +8, +30, +40, +35,100, 7,85,(3,2,1), 9,+40, new Rolagem(D6,2)},
                    new object[]{(28,29), +9, +35, +45, +40,100, 8,95,(3,3,1),10,+45, new Rolagem(D20 )}
                };
                TabelaAtributos = new Tabela<int>(VerificaoLinha , Colunas, tabela);

             
            }



            protected Dictionary<string,object> LinhaTabelaAtributo => TabelaAtributos[(int)Valor];
        }
        public class Forca : Atributo
        {
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
