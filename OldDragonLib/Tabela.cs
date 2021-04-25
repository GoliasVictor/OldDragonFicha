using System;
using System.Collections.Generic;
using System.Linq;
using static OldDragon.Dado;
namespace OldDragon
{
 
    public class Tabela<TKey>
    {
        public delegate bool MetodoVerificacaoLinha(TKey key, Dictionary<string,object> Linha); 
        readonly List<Dictionary<string,object>> tabela = new List<Dictionary<string,object>>();
        readonly MetodoVerificacaoLinha VerificarLinha;
        readonly List<string> Colunas;
        public Tabela(MetodoVerificacaoLinha MetodoProcura,string[] Colunas,  object[][] Linhas = null)
        {

            VerificarLinha = MetodoProcura   ?? throw new ArgumentNullException(nameof(MetodoProcura));
            this.Colunas = Colunas?.ToList() ?? throw new ArgumentNullException(nameof(Colunas)); ;
            if (!(Linhas is null))
                InserirVariasLinhas(Linhas); 
        }
        public object Procurar(string Coluna, Predicate<Dictionary<string,object>> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return tabela.Where(Linha => predicate(Linha)).FirstOrDefault()[Coluna];
        }
        public void InserirLinha( params object[] Linha)
        {
            if (Linha is null)
                throw new ArgumentNullException(nameof(Linha));
            if (Linha.Length != Colunas.Count)
                throw new ArgumentOutOfRangeException();
            
            var LinhaNomeada = new Dictionary<string, object>();

            for (int i = 0; i < Linha.Length; i++)
                LinhaNomeada.Add(Colunas[i], Linha[i]);

            tabela.Add(LinhaNomeada);
        }
        public void InserirVariasLinhas( object[][] Linhas)
        {
            if (Linhas is null)
                throw new ArgumentNullException(nameof(Linhas));
            foreach (var Linha in Linhas) 
                InserirLinha(Linha);
        }
        public Dictionary<string, object> this[TKey key]
        { 
            get
            {  
                foreach (var Linha in tabela)
                    if (VerificarLinha(key, Linha))
                        return Linha;
                throw new KeyNotFoundException("Linha não encontrada"); 
            }
        }

    
    }
    public static class Tabelas
    {

        public static bool VerificaoNivel(uint Nivel, Dictionary<string, object> Linha) => Nivel == (int)Linha["Nivel"];
        public static bool VerificaoAtributo(uint i, Dictionary<string, object> Linha)
        {
            if (Linha is null)
                throw new ArgumentNullException(nameof(Linha));

            var Atributo = ((int Min, int Max))Linha["Atributo"];
            return Atributo.Min >= i || Atributo.Max <= i;
        }
        public static Tabela<uint> TabelaAtributos;
        public static string[] CabecarioTabelaAtributos =
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
        public static object[][] CorpoTabelaAtributos = new object[][] {
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
        public static object[][] HomemDeArmas = new object[][] {
                    new object[]{1 ,       0, new Rolagem(D10, 1, 0), (+1 , 0), 16},
                    new object[]{2 ,    2000, new Rolagem(D10, 2, 0), (+2 , 0), 16},
                    new object[]{3 ,    4000, new Rolagem(D10, 3, 0), (+3 , 0), 16},
                    new object[]{4 ,    8000, new Rolagem(D10, 4, 0), (+4 , 0), 15},
                    new object[]{5 ,   16000, new Rolagem(D10, 5, 0), (+5 , 0), 15},
                    new object[]{6 ,   32000, new Rolagem(D10, 6, 0), (+6 , 0), 15},
                    new object[]{7 ,   64000, new Rolagem(D10, 7, 0), (+7 ,+1), 14},
                    new object[]{8 ,  128000, new Rolagem(D10, 8, 0), (+8 ,+2), 14},
                    new object[]{9 ,  256000, new Rolagem(D10, 9, 0), (+9 ,+3), 14},
                    new object[]{10,  304000, new Rolagem(D10, 9,+2), (+10, 4), 13},
                    new object[]{11,  408000, new Rolagem(D10, 9,+2), (+10, 4), 13},
                    new object[]{12,  516000, new Rolagem(D10, 9,+4), (+11, 5), 13},
                    new object[]{13,  632000, new Rolagem(D10, 9,+4), (+11, 5), 12},
                    new object[]{14,  704000, new Rolagem(D10, 9,+5), (+12, 6), 12},
                    new object[]{15,  808000, new Rolagem(D10, 9,+5), (+12, 6), 12},
                    new object[]{16,  916000, new Rolagem(D10, 9,+6), (+13, 7), 11},
                    new object[]{17, 1032000, new Rolagem(D10, 9,+6), (+13, 7), 11},
                    new object[]{18, 1064000, new Rolagem(D10, 9,+7), (+14, 8), 11},
                    new object[]{19, 1128000, new Rolagem(D10, 9,+7), (+14, 8), 10},
                    new object[]{20, 1256000, new Rolagem(D10, 9,+8), (+15, 9), 10 }
                };
        public static object[][] Ladino = new object[][] {
                    new object[]{1 ,       0, new Rolagem(D6, 1, 0), (+1,0), 15},
                    new object[]{2 ,    1250, new Rolagem(D6, 2, 0), (+1,0), 15},
                    new object[]{3 ,    2500, new Rolagem(D6, 3, 0), (+2,0), 15},
                    new object[]{4 ,    5000, new Rolagem(D6, 4, 0), (+2,0), 14},
                    new object[]{5 ,   10000, new Rolagem(D6, 5, 0), (+2,0), 14},
                    new object[]{6 ,   20000, new Rolagem(D6, 6, 0), (+3,0), 14},
                    new object[]{7 ,   40000, new Rolagem(D6, 7, 0), (+3,0), 13},
                    new object[]{8 ,   80000, new Rolagem(D6, 8, 0), (+3,0), 13},
                    new object[]{9 ,  160000, new Rolagem(D6, 9, 0), (+4,0), 13},
                    new object[]{10,  240000, new Rolagem(D6, 9,+1), (+4,0), 12},
                    new object[]{11,  400000, new Rolagem(D6, 9,+1), (+4,0), 12},
                    new object[]{12,  520000, new Rolagem(D6, 9,+2), (+5,0), 12},
                    new object[]{13,  640000, new Rolagem(D6, 9,+2), (+5,0), 11},
                    new object[]{14,  760000, new Rolagem(D6, 9,+2), (+5,0), 11},
                    new object[]{15,  880000, new Rolagem(D6, 9,+3), (+6,0), 11},
                    new object[]{16, 1000000, new Rolagem(D6, 9,+3), (+6,0), 10},
                    new object[]{17, 1120000, new Rolagem(D6, 9,+3), (+6,0), 10},
                    new object[]{18, 1240000, new Rolagem(D6, 9,+4), (+7,0), 10},
                    new object[]{19, 1360000, new Rolagem(D6, 9,+4), (+7,0), 9 },
                    new object[]{20, 1480000, new Rolagem(D6, 9,+4), (+7,0), 9 }
                };
        public static object[][] Mago = new object[][]
        {
                    new object[]{1 ,       0, new Rolagem(D4,1, 0), ( 0,0), 14},
                    new object[]{2 ,    2500, new Rolagem(D4,2, 0), ( 0,0), 14},
                    new object[]{3 ,    5000, new Rolagem(D4,3, 0), (+1,0), 14},
                    new object[]{4 ,   10000, new Rolagem(D4,4, 0), (+1,0), 13},
                    new object[]{5 ,   20000, new Rolagem(D4,5, 0), (+2,0), 13},
                    new object[]{6 ,   40000, new Rolagem(D4,6, 0), (+2,0), 13},
                    new object[]{7 ,   80000, new Rolagem(D4,7, 0), (+3,0), 12},
                    new object[]{8 ,  160000, new Rolagem(D4,8, 0), (+3,0), 12},
                    new object[]{9 ,  310000, new Rolagem(D4,9, 0), (+3,0), 12},
                    new object[]{10,  460000, new Rolagem(D4,9,+1), (+4,0), 11},
                    new object[]{11,  510000, new Rolagem(D4,9,+1), (+4,0), 11},
                    new object[]{12,  660000, new Rolagem(D4,9,+1), (+4,0), 11},
                    new object[]{13,  710000, new Rolagem(D4,9,+1), (+5,0), 10},
                    new object[]{14,  860000, new Rolagem(D4,9,+1), (+5,0), 10},
                    new object[]{15,  910000, new Rolagem(D4,9,+2), (+5,0), 10},
                    new object[]{16, 1060000, new Rolagem(D4,9,+2), (+6,0), 9 },
                    new object[]{17, 1110000, new Rolagem(D4,9,+2), (+6,0), 9 },
                    new object[]{18, 1160000, new Rolagem(D4,9,+2), (+6,0), 9 },
                    new object[]{19, 1210000, new Rolagem(D4,9,+2), (+7,0), 8 },
                    new object[]{20, 1260000, new Rolagem(D4,9,+3), (+7,0), 8 }
        };
        public static object[][] Clerigo = new object[][]
        {
                   new object[]{ 1 ,       0, new Rolagem(D8,1, 0), (+1,0), 15},
                   new object[]{ 2 ,    1500, new Rolagem(D8,2, 0), (+1,0), 15},
                   new object[]{ 3 ,    3000, new Rolagem(D8,3, 0), (+2,0), 15},
                   new object[]{ 4 ,    6000, new Rolagem(D8,4, 0), (+2,0), 14},
                   new object[]{ 5 ,   12000, new Rolagem(D8,5, 0), (+2,0), 14},
                   new object[]{ 6 ,   24000, new Rolagem(D8,6, 0), (+3,0), 14},
                   new object[]{ 7 ,   48000, new Rolagem(D8,7, 0), (+3,0), 13},
                   new object[]{ 8 ,  100000, new Rolagem(D8,8, 0), (+3,0), 13},
                   new object[]{ 9 ,  200000, new Rolagem(D8,9, 0), (+4,0), 13},
                   new object[]{ 10,  300000, new Rolagem(D8,9,+1), (+4,0), 12},
                   new object[]{ 11,  400000, new Rolagem(D8,9,+1), (+4,0), 12},
                   new object[]{ 12,  500000, new Rolagem(D8,9,+2), (+5,0), 12},
                   new object[]{ 13,  600000, new Rolagem(D8,9,+2), (+5,0), 11},
                   new object[]{ 14,  700000, new Rolagem(D8,9,+3), (+5,0), 11},
                   new object[]{ 15,  800000, new Rolagem(D8,9,+3), (+6,0), 11},
                   new object[]{ 16,  900000, new Rolagem(D8,9,+4), (+6,0), 10},
                   new object[]{ 17, 1000000, new Rolagem(D8,9,+4), (+6,0), 10},
                   new object[]{ 18, 1100000, new Rolagem(D8,9,+5), (+7,0), 10},
                   new object[]{ 19, 1200000, new Rolagem(D8,9,+5), (+7,0), 9 },
                   new object[]{ 20, 1300000, new Rolagem(D8,9,+6), (+7,0), 9 },
        };
        public static uint[][] MagiasClerigo = new uint[][]{
                    new uint[]{1, 0, 0, 0, 0, 0, 0},
                    new uint[]{2, 0, 0, 0, 0, 0, 0},
                    new uint[]{2, 1, 0, 0, 0, 0, 0},
                    new uint[]{3, 2, 0, 0, 0, 0, 0},
                    new uint[]{3, 2, 1, 0, 0, 0, 0},
                    new uint[]{3, 3, 2, 0, 0, 0, 0},
                    new uint[]{4, 3, 2, 1, 0, 0, 0},
                    new uint[]{4, 3, 3, 2, 0, 0, 0},
                    new uint[]{4, 4, 3, 2, 1, 0, 0},
                    new uint[]{5, 4, 3, 3, 2, 0, 0},
                    new uint[]{5, 4, 4, 3, 2, 1, 0},
                    new uint[]{5, 5, 4, 3, 3, 2, 0},
                    new uint[]{6, 5, 4, 4, 3, 2, 0},
                    new uint[]{6, 5, 5, 4, 3, 3, 0},
                    new uint[]{7, 6, 5, 4, 4, 3, 1},
                    new uint[]{7, 6, 5, 5, 4, 3, 2},
                    new uint[]{8, 7, 6, 5, 4, 4, 2},
                    new uint[]{8, 7, 6, 5, 5, 4, 3},
                    new uint[]{9, 8, 7, 6, 5, 4, 3},
                    new uint[]{9, 8, 7, 6, 5, 5, 3}
                };
        public static uint[][] MagiasMago = new uint[][]{
                    new uint[]{1, 0, 0, 0, 0, 0, 0, 0, 0},
                    new uint[]{2, 0, 0, 0, 0, 0, 0, 0, 0},
                    new uint[]{2, 1, 0, 0, 0, 0, 0, 0, 0},
                    new uint[]{2, 2, 0, 0, 0, 0, 0, 0, 0},
                    new uint[]{2, 2, 1, 0, 0, 0, 0, 0, 0},
                    new uint[]{3, 2, 2, 0, 0, 0, 0, 0, 0},
                    new uint[]{3, 2, 2, 1, 0, 0, 0, 0, 0},
                    new uint[]{3, 3, 2, 2, 0, 0, 0, 0, 0},
                    new uint[]{3, 3, 2, 2, 1, 0, 0, 0, 0},
                    new uint[]{3, 3, 3, 2, 2, 0, 0, 0, 0},
                    new uint[]{4, 3, 3, 2, 2, 1, 0, 0, 0},
                    new uint[]{4, 3, 3, 3, 2, 2, 0, 0, 0},
                    new uint[]{4, 4, 3, 3, 2, 2, 1, 0, 0},
                    new uint[]{4, 4, 3, 3, 3, 2, 2, 0, 0},
                    new uint[]{5, 4, 4, 3, 3, 2, 2, 1, 0},
                    new uint[]{5, 4, 4, 3, 3, 3, 2, 2, 0},
                    new uint[]{5, 5, 4, 4, 3, 3, 2, 2, 1},
                    new uint[]{5, 5, 4, 4, 3, 3, 3, 2, 2},
                    new uint[]{5, 5, 5, 4, 4, 3, 3, 2, 2},
                    new uint[]{6, 6, 5, 4, 4, 3, 3, 3, 2},
                };
        public static int n = -3;
        public static int A = -4;
        public static int d = -5;
        public static Tabela<uint> AfastarDadosvidaMortoVivo;
        public static string[] CabecarioAfastarDadosVidaMortoVivo = new string[] { "Nivel", " 1", " 2", " 3", " 4", " 5", " 6", " 7", " 8", " 9", "10", "11", "12", "13", "14", "15", "16", "17", "18" };
        public static object[][] CorpoAfastarDadosvidaMortoVivo = new object[][] {
                    new object[]{ 1, 13, 17, 19,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n},
                    new object[]{ 2, 11, 15, 18, 20,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n},
                    new object[]{ 3,  9, 13, 17, 19,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n},
                    new object[]{ 4,  7, 11, 15, 18, 20,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n},
                    new object[]{ 5,  5,  9, 13, 17, 19,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n},
                    new object[]{ 6,  3,  7, 11, 15, 18, 20,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n},
                    new object[]{ 7,  A,  5,  9, 13, 17, 19,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n},
                    new object[]{ 8,  A,  3,  7, 11, 15, 18, 20,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n},
                    new object[]{ 9,  A,  2,  5,  9, 13, 17, 19,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n},
                    new object[]{10,  d,  A,  3,  7, 11, 15, 18, 20,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n},
                    new object[]{11,  d,  A,  2,  5,  9, 13, 17, 19, 20,  n,  n,  n,  n,  n,  n,  n,  n,  n},
                    new object[]{12,  d,  A,  A,  3,  7, 11, 15, 18, 19, 20,  n,  n,  n,  n,  n,  n,  n,  n},
                    new object[]{13,  d,  d,  A,  2,  5,  9, 13, 17, 18, 19, 20,  n,  n,  n,  n,  n,  n,  n},
                    new object[]{14,  d,  d,  A,  A,  3,  7, 11, 15, 17, 18, 19, 20,  n,  n,  n,  n,  n,  n},
                    new object[]{15,  d,  d,  d,  A,  2,  5,  9, 13, 15, 17, 18, 19, 20,  n,  n,  n,  n,  n},
                    new object[]{16,  d,  d,  d,  A,  A,  3,  7, 11, 13, 15, 17, 18, 19, 20,  n,  n,  n,  n},
                    new object[]{17,  d,  d,  d,  d,  A,  2,  5,  9, 11, 13, 15, 17, 18, 19, 20,  n,  n,  n},
                    new object[]{18,  d,  d,  d,  d,  A,  A,  3,  7,  9, 11, 13, 15, 17, 18, 19, 20,  n,  n},
                    new object[]{19,  d,  d,  d,  d,  d,  A,  2,  5,  7,  9, 11, 13, 15, 17, 18, 19, 20,  n},
                    new object[]{20,  d,  d,  d,  d,  d,  A,  A,  3,  5,  7,  9, 11, 13, 15, 17, 18, 19, 20},
                };
        public static Tabela<uint> TaletosLadrao;
        public static string[] CabecarioTalentosLadrao = new string[] { "Nível", "Arrombar", "Armadilhas", "Escalar", "Furtividade", "Punga", "Percepção", "Ataquefurtivo" };
        public static object[][] CorpoTalentosLadrao = new object[][] {
                    new object[]{ 1, (15,new Rolagem(D8   )), (10,new Rolagem(D8   )), 80, 20, 20, 2,2},
                    new object[]{ 2, (20,new Rolagem(D8   )), (15,new Rolagem(D8   )), 81, 25, 25, 2,2},
                    new object[]{ 3, (25,new Rolagem(D8   )), (20,new Rolagem(D8   )), 82, 30, 30, 2,2},
                    new object[]{ 4, (30,new Rolagem(D8   )), (25,new Rolagem(D8   )), 83, 35, 35, 2,2},
                    new object[]{ 5, (35,new Rolagem(D8   )), (30,new Rolagem(D8   )), 84, 40, 40, 3,2},
                    new object[]{ 6, (40,new Rolagem(D6   )), (35,new Rolagem(D6   )), 85, 45, 45, 3,3},
                    new object[]{ 7, (45,new Rolagem(D6   )), (40,new Rolagem(D6   )), 86, 50, 50, 3,3},
                    new object[]{ 8, (50,new Rolagem(D6   )), (45,new Rolagem(D6   )), 87, 55, 55, 3,3},
                    new object[]{ 9, (55,new Rolagem(D6   )), (50,new Rolagem(D6   )), 88, 60, 60, 3,3},
                    new object[]{10, (60,new Rolagem(D6   )), (55,new Rolagem(D6   )), 89, 65, 65, 4,3},
                    new object[]{11, (62,new Rolagem(D4   )), (60,new Rolagem(D4   )), 90, 70, 70, 4,3},
                    new object[]{12, (64,new Rolagem(D4   )), (62,new Rolagem(D4   )), 91, 72, 72, 4,4},
                    new object[]{13, (66,new Rolagem(D4   )), (64,new Rolagem(D4   )), 92, 74, 74, 4,4},
                    new object[]{14, (68,new Rolagem(D4   )), (66,new Rolagem(D4   )), 93, 76, 76, 4,4},
                    new object[]{15, (70,new Rolagem(D4   )), (68,new Rolagem(D4   )), 94, 78, 78, 4,4},
                    new object[]{16, (72,new Rolagem(0,0,1)), (70,new Rolagem(0,0,1)), 95, 80, 80, 5,4},
                    new object[]{17, (74,new Rolagem(0,0,1)), (72,new Rolagem(0,0,1)), 96, 82, 82, 5,4},
                    new object[]{18, (76,new Rolagem(0,0,1)), (74,new Rolagem(0,0,1)), 97, 84, 84, 5,5},
                    new object[]{19, (78,new Rolagem(0,0,1)), (76,new Rolagem(0,0,1)), 98, 86, 86, 5,5},
                    new object[]{20, (80,new Rolagem(0,0,1)), (78,new Rolagem(0,0,1)), 99, 88, 88, 5,5},
                };
        public static Tabela<uint> CapacidadeCarga = new Tabela<uint>(
            (x, Linha) =>   (int)Linha["ForcaMax"] >= x && x >= (int)Linha["ForcaMin"],
            new string[] { "ForcaMin", "ForcaMax", "CargaLeve", "CargaPesada", "CargaMaxima" },
            new object[][]{
                new object[]{0 ,3  , 10 , 20  , 30  },
                new object[]{4 ,8  , 20 , 30  , 50  },
                new object[]{9 ,12 , 30 , 50  , 70  },
                new object[]{13,15 , 40 , 70  , 90  },
                new object[]{16,17 , 50 , 80  , 100 },
                new object[]{18,20 , 60 , 100 , 120 },
                new object[]{20,22 , 90 , 130 , 150 },
            }); 
        
        static Tabelas()
        {
            AfastarDadosvidaMortoVivo = new Tabela<uint>(VerificaoNivel   , CabecarioAfastarDadosVidaMortoVivo, CorpoAfastarDadosvidaMortoVivo);
            TaletosLadrao             = new Tabela<uint>(VerificaoNivel   , CabecarioTalentosLadrao           , CorpoTalentosLadrao);
            TabelaAtributos           = new Tabela<uint>(VerificaoAtributo, CabecarioTabelaAtributos          , CorpoTabelaAtributos);

        }
    }



}
