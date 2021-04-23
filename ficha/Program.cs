using Newtonsoft.Json;
using OldDragon.Atributo;
using OldDragon.Itens;
using OldDragon.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OldDragon.Dado;

namespace OldDragon
{
   
    class Raca
    {
        public string Nome { get; }
        public uint Movimento { get; }
        public  ModAtributo ModAtributos { get; }
        Raca(string nome, uint movimento, ModAtributo Mod)
        {
            Nome = nome;
            Movimento = movimento;
            ModAtributos = Mod;

        }

        public static Dictionary<string,Raca> Racas =  new Dictionary<string, Raca>(){
            {"Humano"       ,new Raca("Humano"   ,9,new ModAtributo())} ,
            {"Anao"         ,new Raca("Anão"     ,6,new ModAtributo(Con: 2,Car:-2))} ,
            {"Elfo"         ,new Raca("Elfo"     ,9,new ModAtributo(Des: 2,Con:-2))} ,
            {"ElfoNegro"    ,new Raca("Elfo Negro",9,new ModAtributo(Des: 2,Int:2,Sab:-1, Con:-1,Car:-1))} ,
            {"Halfling"     ,new Raca("Halfing"  ,6,new ModAtributo(For:-2,Des: 2))}
        }; 
    }
   

    class Personagem
    {

        public string Nome;
        public Raca Raca;
        public uint XP;
        public int DadosVida;
        public IClasse Classe;
        public Iventario Iventario = new Iventario();
        public List<int> BonusCAOutros = new List<int>();
        [JsonIgnore]
        public Atributos AtributosBase; 
        public Atributos Atributos => AtributosBase + Raca.ModAtributos;
        public int CA
        {
            get
            {
                var CA = 10;
                int BonusDes = Atributos.Des.Ajuste; 
                var ItensProtecao = from Item in Iventario.Itens
                                    where Item.item is Protecao && ((Protecao)Item.item).Usando
                                    select (Protecao)Item.item;

                foreach (Protecao protecao in ItensProtecao)
                {
                    CA += protecao.BonusCa;
                    if (!(protecao.BonuxMaxDes is  null) && BonusDes > protecao.BonuxMaxDes)
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
                                               where Item.item is Protecao
                                               select ((Protecao)Item.item).ReducaoMovimento).Sum();
            }
        }
        public uint Nivel => Classe.Nivel(XP);
        //atributos 
      
        
        public Personagem(string nome, Raca raca, IClasse classe, uint xp, int? dadosVida = null, Atributos? atributos = null)
        {
            Nome = nome;
            Raca = raca;
            Classe = classe;
            XP = xp;
            DadosVida = dadosVida is null ? Classe.DadoVida.Rolar(Nivel).Sum() : (int)dadosVida;
            AtributosBase = atributos is  null ? Atributos.GerarAtributos() : (Atributos)atributos ;
        } 

    } 
    class Program
    {
        static void Main(string[] args)
        {
            Personagem personagem = new Personagem("dougras", Raca.Racas["ElfoNegro"], new Mago(), 30000, atributos: new Atributos(10, 14, 11, 16, 9, 7));
            personagem.Iventario.Itens.AddRange(new List<ItemIventario>() {
                new Protecao("Armadura de placas", 13, 7, 3, -2, true, 300),
                //new Protecao("Escudo madeira", 2, 1, preco: 3),
                new Arma("Adaga", 1, D4, TamanhoArma.G, "CO/PE"),
                //new Arma("Espada", 1, D8, TamanhoArma.M, "CO"),
            });
            personagem.AtributosBase.Des.Valor = 20;
            Console.WriteLine(JsonConvert.SerializeObject(personagem, Formatting.Indented));
            Console.ReadLine();
        }
    }
}
