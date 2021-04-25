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
                DadosClasseNivel = DadosClasseNivel.Concat(tabela.Value[Nivel])
                                                   .ToDictionary((k) => k.Key, v => v.Value);
            return DadosClasseNivel;
        }
        public Classe(string Nome, object[][] TabelaClasse, params (string Nome, Tabela<uint> tabela)[] OutrasTabelas)
        {
            if (string.IsNullOrWhiteSpace(Nome))
                throw new ArgumentException("Nome em branco ou nulo", nameof(Nome));

            if (TabelaClasse is null)
                throw new ArgumentNullException(nameof(TabelaClasse));

            this.Nome = Nome;
            this.TabelaClasse.InserirVariasLinhas(TabelaClasse);
            foreach (var Tabela in OutrasTabelas)
                this.OutrasTabelas.Add(Tabela.Nome, Tabela.tabela);
        } 
    }
    public static class Classes
    {
        public static readonly Classe HomemDeArmas = new Classe("HomemDeArmas", Tabelas.HomemDeArmas);
        public static readonly Classe Ladino = new Classe("Ladino", Tabelas.Ladino, ("TalentosLadrao", Tabelas.TaletosLadrao));
        public static readonly ClasseUsuariaMagia Clerigo = new ClasseUsuariaMagia("Clerigo", Tabelas.Clerigo, Tabelas.MagiasClerigo, ("AfastarDadosvidaMortoVivo", Tabelas.AfastarDadosvidaMortoVivo));
        public static readonly ClasseUsuariaMagia Mago = new ClasseUsuariaMagia("Mago", Tabelas.Mago, Tabelas.MagiasMago);
    }
    public class ClasseUsuariaMagia : Classe
    {
        public uint[][] TabelaMagias;
        public uint[] QuantideMagiaDia(uint Nivel) => TabelaMagias[Nivel - 1];
        public uint QuantideMagiaDia(uint Nivel, uint Circulo) => TabelaMagias[Nivel - 1][Circulo - 1];

        public ClasseUsuariaMagia(string Nome, object[][] TabelaClasse, uint[][] TabelaMagias, params (string Nome, Tabela<uint> tabela)[] OutrasTabelas)
            : base(Nome, TabelaClasse, OutrasTabelas)
        {
            this.TabelaMagias = TabelaMagias ?? throw new ArgumentNullException(nameof(TabelaMagias));
        }
    }
    public class Especializacao
    {
        public string Nome;
        public string Descricao;
        public Alinhamento? Alinhamento;
        public Classe Classe;
        public Especializacao(string nome, Classe classe, string descricao = default, Alinhamento? alinhamento = default)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("message", nameof(nome));
            
            Nome = nome;
            Classe = classe ?? throw new ArgumentNullException(nameof(classe));
            Descricao = descricao;
            Alinhamento = alinhamento;
        }
        public static readonly Dictionary<string, Especializacao> Especializacoes = new Dictionary<string, Especializacao>(){
            { "Druida"      , new Especializacao("Druida"     , Classes.Clerigo)},
            { "Cultista"    , new Especializacao("Cultista"   , Classes.Clerigo, alinhamento: OldDragon.Alinhamento.Caotico)},
            { "Cacador"     , new Especializacao("Cacador"    , Classes.Clerigo)},
            { "Paladino"    , new Especializacao("Paladino"   , Classes.HomemDeArmas,alinhamento: OldDragon.Alinhamento.Ordeiro)},
            { "Guerreiro"   , new Especializacao("Guerreiro"  , Classes.HomemDeArmas)},
            { "Barbaro"     , new Especializacao("Barbaro"    , Classes.HomemDeArmas)},
            { "Ranger"      , new Especializacao("Ranger"     , Classes.Ladino)},
            { "Bardo"       , new Especializacao("Bardo"      , Classes.Ladino)},
            { "Assassino"   , new Especializacao("Assassino"  , Classes.Ladino)},
            { "Ilusionista" , new Especializacao("Ilusionista", Classes.Mago)},
            { "Necromante"  , new Especializacao("Necromante" , Classes.Mago, alinhamento: OldDragon.Alinhamento.Ordeiro)},
            { "Adivinhador" , new Especializacao("Adivinhador", Classes.Mago)}
        };
    }
}