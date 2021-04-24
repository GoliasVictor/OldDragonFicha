using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldDragon
{
    public class Classe
    {
        protected Tabela<uint> TabelaClasse = new Tabela<uint>(Tabelas.VerificaoNivel, new string[] { "Nivel", "XP", "DV", "BA", "JP" });
        protected Dictionary<string, Tabela<uint>> OutrasTabelas = new Dictionary<string, Tabela<uint>>();

        public string Nome { get; }
        public uint Nivel(uint Xp) => (uint)((int)(TabelaClasse.Procurar("Nivel", (Linha) => (int)Linha["XP"] >= Xp)) - 1);
        public uint Experiencia(uint Nivel) => (uint)(int)TabelaClasse[Nivel]["XP"];
        public Rolagem DadoVida(uint Nivel) => (Rolagem)TabelaClasse[Nivel]["DV"];
        public uint JogadaProtecao(uint Nivel) => (uint)(int)TabelaClasse[Nivel]["JP"];
        public (uint BonusMaoPrincipal, uint BonusMaoSecundaria) BaseAtaque(uint Nivel) => ((uint, uint))((int, int))TabelaClasse[Nivel]["BA"];
        public Dictionary<string, object> ObterDadosClasseNivel(uint Nivel)
        {
            Dictionary<string, object> DadosClasseNivel = TabelaClasse[Nivel];
            foreach (var tabela in OutrasTabelas)
                DadosClasseNivel = DadosClasseNivel.Concat(tabela.Value[Nivel]).ToDictionary((k) => k.Key, v => v.Value);
            return DadosClasseNivel;
        }
        public Classe(string Nome, object[][] TabelaClasse, params (string Nome, Tabela<uint> tabela)[] OutrasTabelas)
        {
            this.Nome = Nome;
            this.TabelaClasse.InserirVariasLinhas(TabelaClasse);
            foreach (var Tabela in OutrasTabelas)
                this.OutrasTabelas.Add(Tabela.Nome, Tabela.tabela);
        }

        public static readonly Classe HomemDeArmas = new Classe("HomemDeArmas", Tabelas.HomemDeArmas);
        public static readonly Classe Ladino = new Classe("Ladino", Tabelas.Ladino, ("TalentosLadrao", Tabelas.TaletosLadrao));
        public static readonly ClasseUsuariaMagia Clerigo = new ClasseUsuariaMagia("Clerigo", Tabelas.Clerigo, Tabelas.MagiasClerigo, ("AfastarDadosvidaMortoVivo", Tabelas.AfastarDadosvidaMortoVivo));
        public static readonly ClasseUsuariaMagia Mago    = new ClasseUsuariaMagia("Mago"   , Tabelas.Mago, Tabelas.MagiasMago);
    }
    public class ClasseUsuariaMagia : Classe
    {
        public uint[][] TabelaMagias;
        public uint[] QuantideMagiaDia(uint Nivel) => TabelaMagias[Nivel - 1];
        public uint QuantideMagiaDia(uint Nivel, uint Circulo) => TabelaMagias[Nivel - 1][Circulo - 1];

        public ClasseUsuariaMagia(string Nome, object[][] TabelaClasse, uint[][] TabelaMagias, params (string Nome, Tabela<uint> tabela)[] OutrasTabelas)
            : base(Nome, TabelaClasse, OutrasTabelas)
        {
            this.TabelaMagias = TabelaMagias;
        }
    }
    public class Especializacao
    {
        public string Nome;
        public string Descricao;
        public Alinhamento? Alinhamento;
        public Classe Classe;
        public Especializacao(string Nome, Classe Classe, string Descricao = "", Alinhamento? Alinhamento = null)
        {
            this.Nome = Nome;
            this.Classe = Classe;
            this.Descricao = Descricao;
            this.Alinhamento = Alinhamento;
        }
        public static readonly Dictionary<string, Especializacao> Especializacoes = new Dictionary<string, Especializacao>(){
            { "Druida"      , new Especializacao("Druida"     , Classe.Clerigo)},
            { "Cultista"    , new Especializacao("Cultista"   , Classe.Clerigo, Alinhamento: OldDragon.Alinhamento.Caotico)},
            { "Cacador"     , new Especializacao("Cacador"    , Classe.Clerigo)},
            { "Paladino"    , new Especializacao("Paladino"   , Classe.HomemDeArmas,Alinhamento: OldDragon.Alinhamento.Ordeiro)},
            { "Guerreiro"   , new Especializacao("Guerreiro"  , Classe.HomemDeArmas)},
            { "Barbaro"     , new Especializacao("Barbaro"    , Classe.HomemDeArmas)},
            { "Ranger"      , new Especializacao("Ranger"     , Classe.Ladino)},
            { "Bardo"       , new Especializacao("Bardo"      , Classe.Ladino)},
            { "Assassino"   , new Especializacao("Assassino"  , Classe.Ladino)},
            { "Ilusionista" , new Especializacao("Ilusionista", Classe.Mago)},
            { "Necromante"  , new Especializacao("Necromante" , Classe.Mago, Alinhamento: OldDragon.Alinhamento.Ordeiro)},
            { "Adivinhador" , new Especializacao("Adivinhador", Classe.Mago)}
        };
    }
}