using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;
using static OldDragon.Dado;

namespace OldDragon
{
    namespace Atributo
    {
        struct Atributos
        {
            public Forca For;
            public Destreza Des;
            public Constituicao Con;
            public Inteligencia Int;
            public Sabedoria Sab;
            public Carisma Car;
            public static Atributos GerarAtributos()
            {
                Rolagem rolagem = new Rolagem(D6, 3);
                return new Atributos(rolagem.Rolar(),
                                     rolagem.Rolar(),
                                     rolagem.Rolar(),
                                     rolagem.Rolar(),
                                     rolagem.Rolar(),
                                     rolagem.Rolar());
            }

            public Atributos(int For, int Con, int Des, int Int, int Sab, int Car)
            {
                this.For = new Forca(For);
                this.Des = new Destreza(Des);
                this.Con = new Constituicao(Con);
                this.Int = new Inteligencia(Int);
                this.Sab = new Sabedoria(Sab);
                this.Car = new Carisma(Car);
            }
            public static Atributos operator +(Atributos Atr, ModAtributo Mod)
           => new Atributos(Atr.For + Mod.For,
                            Atr.Con + Mod.Con,
                            Atr.Des + Mod.Des,
                            Atr.Int + Mod.Int,
                            Atr.Sab + Mod.Sab,
                            Atr.Car + Mod.Car);
            public static Atributos operator -(Atributos Atr, ModAtributo Mod)
           => new Atributos(Atr.For - Mod.For,
                            Atr.Con - Mod.Con,
                            Atr.Des - Mod.Des,
                            Atr.Int - Mod.Int,
                            Atr.Sab - Mod.Sab,
                            Atr.Car - Mod.Car);

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
            
            public int Valor { get; set; }
            public int Ajuste => (int)Math.Floor((Valor - 10) / 2.0);
            public Atributo(int valor) => Valor = valor;
            public override string ToString() => Valor.ToString();
            public static implicit operator int(Atributo d) => d.Valor;
            public static Dictionary<int, LinhaAtributos> TabelaAtributos = new Dictionary<int, LinhaAtributos>{
                    {1, new LinhaAtributos(-5, -25, -25, -25,  0, 0, 0,(0,0,0), 0,-25, new Rolagem(0, 0)) },
                    {3, new LinhaAtributos(-4, -20, -20, -20,  0, 0, 0,(0,0,0), 0,-20, new Rolagem(0, 0)) },
                    {5, new LinhaAtributos(-3, -15, -15, -15,  0, 0, 0,(0,0,0), 0,-15, new Rolagem(0, 0)) },
                    {7, new LinhaAtributos(-2, -10, -10, -10,  1, 0, 0,(0,0,0), 0,-10, new Rolagem(0, 0)) },
                    {9, new LinhaAtributos(-1,  -5,  -5,  -5,  5, 1, 5,(0,0,0), 0, -5, new Rolagem(1   )) },
                    {11,new LinhaAtributos( 0,   0,   0,   0, 10, 1,10,(0,0,0), 1,  0, new Rolagem(D2  )) },
                    {13,new LinhaAtributos(+1,   0,  +5,   0, 25, 1,20,(0,0,0), 2, +5, new Rolagem(D3  )) },
                    {15,new LinhaAtributos(+2,   0, +10,  +5, 50, 1,25,(0,0,0), 3,+10, new Rolagem(D4  )) },
                    {17,new LinhaAtributos(+3,  +5, +15, +10, 75, 2,35,(1,0,0), 4,+15, new Rolagem(D6  )) },
                    {19,new LinhaAtributos(+4, +10, +20, +15, 95, 3,45,(2,0,0), 5,+20, new Rolagem(D8  )) },
                    {21,new LinhaAtributos(+5, +15, +25, +20,100, 4,55,(2,1,0), 6,+25, new Rolagem(D4,2)) },
                    {23,new LinhaAtributos(+6, +20, +30, +25,100, 5,65,(2,2,0), 7,+30, new Rolagem(D10 )) },
                    {25,new LinhaAtributos(+7, +25, +35, +30,100, 6,75,(2,2,1), 8,+35, new Rolagem(D12 )) },
                    {27,new LinhaAtributos(+8, +30, +40, +35,100, 7,85,(3,2,1), 9,+40, new Rolagem(D6,2)) },
                    {29,new LinhaAtributos(+9, +35, +45, +40,100, 8,95,(3,3,1),10,+45, new Rolagem(D20 )) }
                };
            protected LinhaAtributos LinhaTabelaAtributo => TabelaAtributos[Valor + 1 - (Valor % 2)];
            public struct LinhaAtributos
            {
                public int Ajuste;
                public int Armadilhas;
                public int Arrombar;
                public int FurtividadEPungar;
                public int ChanceRessureicao;
                public int IdiomasAdicionais;
                public int ChanceAprenderMagia;
                public (int C1, int C2, int C3) MagiasAdicionais;
                public int NumeroSeguidores;
                public int AjusteReacao;
                public Rolagem QtMortoVivoAfastado;
                public LinhaAtributos(int Ajuste,
                               int Armadilhas,
                               int Arrombar,
                               int FurtividadEPungar,
                               int ChanceRessureicao,
                               int IdiomasAdicionais,
                               int ChanceAprenderMagia,
                               (int, int, int) MagiasAdicionais,
                               int NumeroSeguidores,
                               int AjusteReacao,
                               Rolagem QtMortoVivoAfastado)
                {
                    this.Ajuste = Ajuste;
                    this.Armadilhas = Armadilhas;
                    this.Arrombar = Arrombar;
                    this.FurtividadEPungar = FurtividadEPungar;
                    this.ChanceRessureicao = ChanceRessureicao;
                    this.IdiomasAdicionais = IdiomasAdicionais;
                    this.ChanceAprenderMagia = ChanceAprenderMagia;
                    this.MagiasAdicionais = MagiasAdicionais;
                    this.NumeroSeguidores = NumeroSeguidores;
                    this.AjusteReacao = AjusteReacao;
                    this.QtMortoVivoAfastado = QtMortoVivoAfastado;
                }
            }
        }
        public class Forca : Atributo
        {
            public static DataTable Tabela;

            public Forca(int valor) : base(valor) {}
        }

        public class Destreza : Atributo
        { 
            
            public int Armadilhas => LinhaTabelaAtributo.Armadilhas;
            public int Arrombar => LinhaTabelaAtributo.Arrombar;
            public int Furtividade => LinhaTabelaAtributo.FurtividadEPungar; 
            public int Pungar => LinhaTabelaAtributo.FurtividadEPungar; 
            public Destreza(int valor) : base(valor) { }
        }
        public class Constituicao : Atributo
        {

            public int ChanceRessureicao => LinhaTabelaAtributo.ChanceRessureicao;
            public Constituicao(int valor) : base(valor) { }
        }
        public class Inteligencia : Atributo
        {
            public int IdiomasAdicionais => LinhaTabelaAtributo.IdiomasAdicionais;
            public int ChanceAprenderMagia => LinhaTabelaAtributo.ChanceAprenderMagia;
            public Inteligencia(int valor) : base(valor) { }
        }
        public class Sabedoria : Atributo
        {
            public (int C1, int C2, int C3) MagiasAdicionais => LinhaTabelaAtributo.MagiasAdicionais;
            public Sabedoria(int valor) : base(valor) { }
        }
        public class Carisma : Atributo
        {
            public int NumeroMaxSeguidores => LinhaTabelaAtributo.NumeroSeguidores;
            public int AjusteReacao => LinhaTabelaAtributo.AjusteReacao;
            public Rolagem QtMortoVivoAfastado => LinhaTabelaAtributo.QtMortoVivoAfastado;
            public Carisma(int valor) : base(valor) { }
        }
    }
}
