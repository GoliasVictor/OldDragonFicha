using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OldDragon.Atributo;
using OldDragon.Itens;
using OldDragon.Classes;

namespace OldDragon
{
    public class Raca
    {
        public string Nome { get; }
        public uint Movimento { get; }
        public ModAtributo ModAtributos { get; }
        Raca(string nome, uint movimento, ModAtributo Mod)
        {
            Nome = nome;
            Movimento = movimento;
            ModAtributos = Mod;

        }

        public static Dictionary<string, Raca> Racas = new Dictionary<string, Raca>(){
            {"Humano"       ,new Raca("Humano"   ,9,new ModAtributo())} ,
            {"Anao"         ,new Raca("Anão"     ,6,new ModAtributo(Con: 2,Car:-2))} ,
            {"Elfo"         ,new Raca("Elfo"     ,9,new ModAtributo(Des: 2,Con:-2))} ,
            {"ElfoNegro"    ,new Raca("Elfo Negro",9,new ModAtributo(Des: 2,Int:2,Sab:-1, Con:-1,Car:-1))} ,
            {"Halfling"     ,new Raca("Halfing"  ,6,new ModAtributo(For:-2,Des: 2))}
        };
    }
     
    public class Personagem
    {

        public string Nome;
        public Raca Raca;
        public uint XP;
        public int DadosVida;
        public Classe Classe;
        public Iventario Iventario = new Iventario();
        public List<int> BonusCAOutros = new List<int>();
        public uint Nivel => Classe.Nivel(XP);
        public uint JP => Classe.JogadaProtecao(Nivel);
        public (uint BonusMaoPrincipal, uint BonusMaoSecundaria) BA => Classe.BaseAtaque(Nivel); 
        public Atributos AtributosBase { get; private set;}
        public Atributos Atributos { get => AtributosBase + Raca.ModAtributos;   set => AtributosBase = value - Raca.ModAtributos; }
        public int CA
        {
            get 
            {
                
                var CA = 10;
                int BonusDes = Atributos.Des.Ajuste;
                var ItensProtecao = from Item in Iventario.Itens
                                    where Item is Protecao && ((Protecao)Item).Usando
                                    select (Protecao)Item;

                foreach (Protecao protecao in ItensProtecao)
                {
                    CA += protecao.BonusCa;
                    if (!(protecao.BonuxMaxDes is null) && BonusDes > protecao.BonuxMaxDes)
                        BonusDes = (int)protecao.BonuxMaxDes;
                }
                return CA + BonusDes + BonusCAOutros.Sum();
            }
        }
        public uint Movimento
        {
            get
            {
                return Raca.Movimento + (uint)(from Item in Iventario.Itens
                                               where Item is Protecao
                                               select ((Protecao)Item).ReducaoMovimento).Sum();
            }
        }
        //atributos 
          
        public Personagem(string nome, Raca raca, Classe classe, uint xp, int? dadosVida = null, Atributos? atributos = null)
        {
            Nome = nome;
            Raca = raca;
            Classe = classe;
            XP = xp;
            DadosVida = dadosVida is null ? Classe.DadoVida(Nivel).Rolar() : (int)dadosVida;
            AtributosBase = atributos is null ? Atributos.GerarAtributos() : (Atributos)atributos;
        }

    }
}
