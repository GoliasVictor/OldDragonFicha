using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OldDragon.Atributo;
using OldDragon.Itens; 

namespace OldDragon
{
    public enum Alinhamento {Ordeiro,Neutro,Caotico}
    public enum Carga { SemCarga, CargaLeve,CargaPesada,CargaMaxima}
    public class Raca
    {
        public string Nome { get; }
        public uint Movimento { get; }
        public ModAtributo ModAtributos { get; }
        Raca(string nome, uint movimento, ModAtributo Mod)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("message", nameof(nome));

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
     
   
}
