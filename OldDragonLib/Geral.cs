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
     
    public class Personagem
    {

        public string Nome;
        public Raca Raca;
        public uint XP;
        public int DadosVida;
        public Alinhamento Alinhamento;
        public Classe Classe;
        public Atributos AtributosBase;
        public Iventario Iventario = new Iventario();
        public List<int> BonusCAOutros = new List<int>();
        public uint Nivel => Classe.Nivel(XP);
        public Dado DV => Classe.DadoVida(Nivel).Dado;
        public int PV => DadosVida + Atributos.Con.Ajuste;
        public uint JP => Classe.JogadaProtecao(Nivel);
        public (uint BonusMaoPrincipal, uint BonusMaoSecundaria) BA => Classe.BaseAtaque(Nivel);
        public Atributos Atributos { get => AtributosBase + Raca.ModAtributos;   set => AtributosBase = value - Raca.ModAtributos; }
        public int CA
        {
            get 
            {
                
                var CA = 10;
                int BonusDes = Atributos.Des.Ajuste;
                var ItensProtecao = from Item in Iventario
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
                if (CapacidadeCarga == Carga.CargaMaxima)
                    return 1;

                return  (uint)(Raca.Movimento + (int)RedMovimento
                    + ((uint)Iventario.Where(Item => Item is Protecao).Select(Item => ((Protecao)Item)?.ReducaoMovimento ?? 0).Sum()));
            }
        }
        public Carga CapacidadeCarga
        {
            get
            {
                var (CargaLeve, CargaPesada, CargaMaxima) = Atributos.For.CapacidadeCarga;
                var PesoTotal = Iventario.PesoTotal;
                if (PesoTotal >= CargaMaxima) return Carga.CargaMaxima;
                if (PesoTotal >= CargaPesada) return Carga.CargaPesada;
                if (PesoTotal >= CargaLeve  ) return Carga.CargaLeve;
                else return Carga.SemCarga;
            }
        }
        public int? RedMovimento
        {
            get
            {
                switch (CapacidadeCarga)
                {
                    case Carga.CargaLeve:  return 0;
                    case Carga.SemCarga:   return -1;
                    case Carga.CargaPesada:return -2;
                    case Carga.CargaMaxima:
                    default: return null;
                }
            }
        }


        public Personagem(string nome, Raca raca, Classe classe, uint xp, Alinhamento alinhamento, int? dadosVida = null,
                          Atributos? atributos = null, Iventario iventario = default)
        {
            if (string.IsNullOrWhiteSpace(nome))
                 throw new ArgumentException("Nome nulo ou em branco.", nameof(nome));
            Nome = nome;
            Raca = raca ?? throw new ArgumentNullException(nameof(raca));
            Classe = classe ?? throw new ArgumentNullException(nameof(classe));
            XP = xp;
            Alinhamento = alinhamento;
            DadosVida = dadosVida ?? Classe.DadoVida(Nivel).Rolar();
            AtributosBase = atributos ?? Atributos.GerarAtributos();
            Iventario = iventario ?? new Iventario();
        }

    }
}
