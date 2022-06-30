using System;
using System.Diagnostics;
using static OldDragon.Dado;

namespace OldDragon
{
	public record struct VlCarga (uint Leve,uint Pesada,uint Maxima);

	public static class Tabelas
	{
		
		class ConfigChaveIntervalo<TLinha> : ConfiguracaoTabela<uint,MinMax,TLinha> 
			where TLinha : ILinhaChaveIntervalo
		{
			
			private bool DentroIntervalo(uint i, MinMax Intervalo){
				return Intervalo.Min <= i &&  i <= Intervalo.Max;
			} 
			public override bool CompararChaveLinha(uint i, TLinha Linha)
			{ 
				return DentroIntervalo(i, Linha.key);
			}
			public override bool CompararLinhas(TLinha A, TLinha B)
			{
				return DentroIntervalo(A.key.Min, B.key)
					|| DentroIntervalo(A.key.Max, B.key)
					|| DentroIntervalo(B.key.Min, A.key)
					|| DentroIntervalo(B.key.Max, A.key);
			}
		} 
		public record struct MinMax (uint Min, uint Max);  
		interface ILinhaChaveIntervalo : ILinhaTabela<MinMax> {

		}
		public static bool VerificaoCapacidadeCarga(uint i, LCapacidadeCarga Linha)
		{  
			return Linha.Forca.Min <= i && i <= Linha.Forca.Max  ;
		}
		
		public record struct LTabelaAtributos(
			MinMax Atributo,
			int Ajuste,
			int Armadilhas,
			int Arrombar,
			int FurtividadEPungar,
			int ChanceRessureicao,
			int IdiomasAdicionais,
			int ChanceAprenderMagia,
			(uint C1, uint C2,uint C3) MagiasAdicionais,
			int NumeroSeguidores,
			int AjusteReacao,
			Rolagem QtMortoVivoAfastado
		)  : ILinhaChaveIntervalo
		{
			public MinMax key => Atributo;
		}
		public record struct LTabelaClasse (		
			uint Nivel,
			uint XP,
			Rolagem DV,
			(uint, uint) BA,
			uint JP 
		): ILinhaTabela<uint>
		{
			public uint key => Nivel;
		}
		public static Tabela<uint,MinMax,LTabelaAtributos> TabelaAtributos;
		public static Tabela<uint,LTabelaClasse>  HomemDeArmas;
		public static Tabela<uint,LTabelaClasse> Ladino;
		public static Tabela<uint,LTabelaClasse> Clerigo;
		public static Tabela<uint,LTabelaClasse> Mago;
		public static Tabela<uint,LQtMagias> MagiasClerigo;
		public static Tabela<uint,LQtMagias> MagiasMago;
        public static Tabela<uint,LAfastarDadosvidaMortoVivo> AfastarDadosvidaMortoVivo ;
		public static Tabela<uint,LTalentosLadrao> TalentosLadrao;
		public struct LQtMagias : ILinhaTabela<uint>{
			public uint key => Nivel;
			public uint Nivel;
			public readonly uint[] Circulos;
			public LQtMagias(uint nivel,uint[] circulos)
			{
				Nivel = nivel;
				Circulos = circulos;
			}
		}
        public record struct LTalentosLadrao ( 
			uint Nivel,
			(uint, Rolagem) Arrombar,
			(uint, Rolagem) Armadilhas,
			uint Escalar,
			uint Furtividade,
			uint Punga,
			uint Percepção,
			uint Ataquefurtivo
		) : ILinhaTabela<uint>
		{
			public uint key => Nivel;
		}
		
		public record struct LCapacidadeCarga (
			MinMax Forca,
			VlCarga Carga
		) : ILinhaChaveIntervalo
		{ 
			public MinMax key => Forca;
		} 
		public record struct LAfastarDadosvidaMortoVivo : ILinhaTabela<uint>{
			public uint key => Nivel;
			public uint Nivel;
			public readonly int[] Dificudades;

			public LAfastarDadosvidaMortoVivo(uint nivel,int[] dificudades)
			{
				if(dificudades.Length != 19)
				 throw new Exception("Tamanho errado");
				Nivel = nivel;
				Dificudades = dificudades;
			}
		}
		public static Tabela<uint,MinMax,LCapacidadeCarga> TabelaCapacidadeCarga;
		static Tabelas(){
			var CorpoAtributos = new LTabelaAtributos[] 
			{
				new LTabelaAtributos(new( 0, 1), -5, -25, -25, -25,  0, 0, 0,(0,0,0), 0,-25, new Rolagem(0, 0)),
				new LTabelaAtributos(new( 2, 3), -4, -20, -20, -20,  0, 0, 0,(0,0,0), 0,-20, new Rolagem(0, 0)),
				new LTabelaAtributos(new( 4, 5), -3, -15, -15, -15,  0, 0, 0,(0,0,0), 0,-15, new Rolagem(0, 0)),
				new LTabelaAtributos(new( 6, 7), -2, -10, -10, -10,  1, 0, 0,(0,0,0), 0,-10, new Rolagem(0, 0)),
				new LTabelaAtributos(new( 8, 9), -1,  -5,  -5,  -5,  5, 1, 5,(0,0,0), 0, -5, new Rolagem(1   )),
				new LTabelaAtributos(new(10,11),  0,   0,   0,   0, 10, 1,10,(0,0,0), 1,  0, new Rolagem(D2  )),
				new LTabelaAtributos(new(12,13), +1,   0,  +5,   0, 25, 1,20,(0,0,0), 2, +5, new Rolagem(D3  )),
				new LTabelaAtributos(new(14,15), +2,   0, +10,  +5, 50, 1,25,(0,0,0), 3,+10, new Rolagem(D4  )),
				new LTabelaAtributos(new(16,17), +3,  +5, +15, +10, 75, 2,35,(1,0,0), 4,+15, new Rolagem(D6  )),
				new LTabelaAtributos(new(18,19), +4, +10, +20, +15, 95, 3,45,(2,0,0), 5,+20, new Rolagem(D8  )),
				new LTabelaAtributos(new(20,21), +5, +15, +25, +20,100, 4,55,(2,1,0), 6,+25, new Rolagem(D4,2)),
				new LTabelaAtributos(new(22,23), +6, +20, +30, +25,100, 5,65,(2,2,0), 7,+30, new Rolagem(D10 )),
				new LTabelaAtributos(new(24,25), +7, +25, +35, +30,100, 6,75,(2,2,1), 8,+35, new Rolagem(D12 )),
				new LTabelaAtributos(new(26,27), +8, +30, +40, +35,100, 7,85,(3,2,1), 9,+40, new Rolagem(D6,2)),
				new LTabelaAtributos(new(28,29), +9, +35, +45, +40,100, 8,95,(3,3,1),10,+45, new Rolagem(D20 ))
			};
			var CorpoHomemDeArmas = new LTabelaClasse[] {
				new LTabelaClasse(1 ,       0, new Rolagem(D10, 1, 0), (+1 , 0), 16),
				new LTabelaClasse(2 ,    2000, new Rolagem(D10, 2, 0), (+2 , 0), 16),
				new LTabelaClasse(3 ,    4000, new Rolagem(D10, 3, 0), (+3 , 0), 16),
				new LTabelaClasse(4 ,    8000, new Rolagem(D10, 4, 0), (+4 , 0), 15),
				new LTabelaClasse(5 ,   16000, new Rolagem(D10, 5, 0), (+5 , 0), 15),
				new LTabelaClasse(6 ,   32000, new Rolagem(D10, 6, 0), (+6 , 0), 15),
				new LTabelaClasse(7 ,   64000, new Rolagem(D10, 7, 0), (+7 ,+1), 14),
				new LTabelaClasse(8 ,  128000, new Rolagem(D10, 8, 0), (+8 ,+2), 14),
				new LTabelaClasse(9 ,  256000, new Rolagem(D10, 9, 0), (+9 ,+3), 14),
				new LTabelaClasse(10,  304000, new Rolagem(D10, 9,+2), (+10, 4), 13),
				new LTabelaClasse(11,  408000, new Rolagem(D10, 9,+2), (+10, 4), 13),
				new LTabelaClasse(12,  516000, new Rolagem(D10, 9,+4), (+11, 5), 13),
				new LTabelaClasse(13,  632000, new Rolagem(D10, 9,+4), (+11, 5), 12),
				new LTabelaClasse(14,  704000, new Rolagem(D10, 9,+5), (+12, 6), 12),
				new LTabelaClasse(15,  808000, new Rolagem(D10, 9,+5), (+12, 6), 12),
				new LTabelaClasse(16,  916000, new Rolagem(D10, 9,+6), (+13, 7), 11),
				new LTabelaClasse(17, 1032000, new Rolagem(D10, 9,+6), (+13, 7), 11),
				new LTabelaClasse(18, 1064000, new Rolagem(D10, 9,+7), (+14, 8), 11),
				new LTabelaClasse(19, 1128000, new Rolagem(D10, 9,+7), (+14, 8), 10),
				new LTabelaClasse(20, 1256000, new Rolagem(D10, 9,+8), (+15, 9), 10)
			};
            var CorpoClerigo = new LTabelaClasse[]{
				new LTabelaClasse(1 ,       0, new Rolagem(D8,1, 0), (+1,0), 15),
				new LTabelaClasse(2 ,    1500, new Rolagem(D8,2, 0), (+1,0), 15),
				new LTabelaClasse(3 ,    3000, new Rolagem(D8,3, 0), (+2,0), 15),
				new LTabelaClasse(4 ,    6000, new Rolagem(D8,4, 0), (+2,0), 14),
				new LTabelaClasse(5 ,   12000, new Rolagem(D8,5, 0), (+2,0), 14),
				new LTabelaClasse(6 ,   24000, new Rolagem(D8,6, 0), (+3,0), 14),
				new LTabelaClasse(7 ,   48000, new Rolagem(D8,7, 0), (+3,0), 13),
				new LTabelaClasse(8 ,  100000, new Rolagem(D8,8, 0), (+3,0), 13),
				new LTabelaClasse(9 ,  200000, new Rolagem(D8,9, 0), (+4,0), 13),
				new LTabelaClasse(10,  300000, new Rolagem(D8,9,+1), (+4,0), 12),
				new LTabelaClasse(11,  400000, new Rolagem(D8,9,+1), (+4,0), 12),
				new LTabelaClasse(12,  500000, new Rolagem(D8,9,+2), (+5,0), 12),
				new LTabelaClasse(13,  600000, new Rolagem(D8,9,+2), (+5,0), 11),
				new LTabelaClasse(14,  700000, new Rolagem(D8,9,+3), (+5,0), 11),
				new LTabelaClasse(15,  800000, new Rolagem(D8,9,+3), (+6,0), 11),
				new LTabelaClasse(16,  900000, new Rolagem(D8,9,+4), (+6,0), 10),
				new LTabelaClasse(17, 1000000, new Rolagem(D8,9,+4), (+6,0), 10),
				new LTabelaClasse(18, 1100000, new Rolagem(D8,9,+5), (+7,0), 10),
				new LTabelaClasse(19, 1200000, new Rolagem(D8,9,+5), (+7,0), 9 ),
				new LTabelaClasse(20, 1300000, new Rolagem(D8,9,+6), (+7,0), 9 ),
		    };
			var CorpoLadino  = new LTabelaClasse[] {
				new LTabelaClasse(1 ,       0, new Rolagem(D6, 1, 0), (+1,0), 15),
				new LTabelaClasse(2 ,    1250, new Rolagem(D6, 2, 0), (+1,0), 15),
				new LTabelaClasse(3 ,    2500, new Rolagem(D6, 3, 0), (+2,0), 15),
				new LTabelaClasse(4 ,    5000, new Rolagem(D6, 4, 0), (+2,0), 14),
				new LTabelaClasse(5 ,   10000, new Rolagem(D6, 5, 0), (+2,0), 14),
				new LTabelaClasse(6 ,   20000, new Rolagem(D6, 6, 0), (+3,0), 14),
				new LTabelaClasse(7 ,   40000, new Rolagem(D6, 7, 0), (+3,0), 13),
				new LTabelaClasse(8 ,   80000, new Rolagem(D6, 8, 0), (+3,0), 13),
				new LTabelaClasse(9 ,  160000, new Rolagem(D6, 9, 0), (+4,0), 13),
				new LTabelaClasse(10,  240000, new Rolagem(D6, 9,+1), (+4,0), 12),
				new LTabelaClasse(11,  400000, new Rolagem(D6, 9,+1), (+4,0), 12),
				new LTabelaClasse(12,  520000, new Rolagem(D6, 9,+2), (+5,0), 12),
				new LTabelaClasse(13,  640000, new Rolagem(D6, 9,+2), (+5,0), 11),
				new LTabelaClasse(14,  760000, new Rolagem(D6, 9,+2), (+5,0), 11),
				new LTabelaClasse(15,  880000, new Rolagem(D6, 9,+3), (+6,0), 11),
				new LTabelaClasse(16, 1000000, new Rolagem(D6, 9,+3), (+6,0), 10),
				new LTabelaClasse(17, 1120000, new Rolagem(D6, 9,+3), (+6,0), 10),
				new LTabelaClasse(18, 1240000, new Rolagem(D6, 9,+4), (+7,0), 10),
				new LTabelaClasse(19, 1360000, new Rolagem(D6, 9,+4), (+7,0), 9 ),
				new LTabelaClasse(20, 1480000, new Rolagem(D6, 9,+4), (+7,0), 9 )
			};
			var CorpoMago = new LTabelaClasse[]
			{
				new LTabelaClasse(1 ,       0, new Rolagem(D4,1, 0), ( 0,0), 14),
				new LTabelaClasse(2 ,    2500, new Rolagem(D4,2, 0), ( 0,0), 14),
				new LTabelaClasse(3 ,    5000, new Rolagem(D4,3, 0), (+1,0), 14),
				new LTabelaClasse(4 ,   10000, new Rolagem(D4,4, 0), (+1,0), 13),
				new LTabelaClasse(5 ,   20000, new Rolagem(D4,5, 0), (+2,0), 13),
				new LTabelaClasse(6 ,   40000, new Rolagem(D4,6, 0), (+2,0), 13),
				new LTabelaClasse(7 ,   80000, new Rolagem(D4,7, 0), (+3,0), 12),
				new LTabelaClasse(8 ,  160000, new Rolagem(D4,8, 0), (+3,0), 12),
				new LTabelaClasse(9 ,  310000, new Rolagem(D4,9, 0), (+3,0), 12),
				new LTabelaClasse(10,  460000, new Rolagem(D4,9,+1), (+4,0), 11),
				new LTabelaClasse(11,  510000, new Rolagem(D4,9,+1), (+4,0), 11),
				new LTabelaClasse(12,  660000, new Rolagem(D4,9,+1), (+4,0), 11),
				new LTabelaClasse(13,  710000, new Rolagem(D4,9,+1), (+5,0), 10),
				new LTabelaClasse(14,  860000, new Rolagem(D4,9,+1), (+5,0), 10),
				new LTabelaClasse(15,  910000, new Rolagem(D4,9,+2), (+5,0), 10),
				new LTabelaClasse(16, 1060000, new Rolagem(D4,9,+2), (+6,0), 9 ),
				new LTabelaClasse(17, 1110000, new Rolagem(D4,9,+2), (+6,0), 9 ),
				new LTabelaClasse(18, 1160000, new Rolagem(D4,9,+2), (+6,0), 9 ),
				new LTabelaClasse(19, 1210000, new Rolagem(D4,9,+2), (+7,0), 8 ),
				new LTabelaClasse(20, 1260000, new Rolagem(D4,9,+3), (+7,0), 8 )
			};
			var CorpoMagiasClerigo = new LQtMagias[]{
				new LQtMagias(1 , new uint[]{1, 0, 0, 0, 0, 0, 0}),
				new LQtMagias(2 , new uint[]{2, 0, 0, 0, 0, 0, 0}),
				new LQtMagias(3 , new uint[]{2, 1, 0, 0, 0, 0, 0}),
				new LQtMagias(4 , new uint[]{3, 2, 0, 0, 0, 0, 0}),
				new LQtMagias(5 , new uint[]{3, 2, 1, 0, 0, 0, 0}),
				new LQtMagias(6 , new uint[]{3, 3, 2, 0, 0, 0, 0}),
				new LQtMagias(7 , new uint[]{4, 3, 2, 1, 0, 0, 0}),
				new LQtMagias(8 , new uint[]{4, 3, 3, 2, 0, 0, 0}),
				new LQtMagias(9 , new uint[]{4, 4, 3, 2, 1, 0, 0}),
				new LQtMagias(10, new uint[]{5, 4, 3, 3, 2, 0, 0}),
				new LQtMagias(11, new uint[]{5, 4, 4, 3, 2, 1, 0}),
				new LQtMagias(12, new uint[]{5, 5, 4, 3, 3, 2, 0}),
				new LQtMagias(13, new uint[]{6, 5, 4, 4, 3, 2, 0}),
				new LQtMagias(14, new uint[]{6, 5, 5, 4, 3, 3, 0}),
				new LQtMagias(15, new uint[]{7, 6, 5, 4, 4, 3, 1}),
				new LQtMagias(16, new uint[]{7, 6, 5, 5, 4, 3, 2}),
				new LQtMagias(17, new uint[]{8, 7, 6, 5, 4, 4, 2}),
				new LQtMagias(18, new uint[]{8, 7, 6, 5, 5, 4, 3}),
				new LQtMagias(19, new uint[]{9, 8, 7, 6, 5, 4, 3}),
				new LQtMagias(20, new uint[]{9, 8, 7, 6, 5, 5, 3})
			};
			var CorpoMagiasMago = new LQtMagias[]{
				new LQtMagias(1 ,new uint[]{1, 0, 0, 0, 0, 0, 0, 0, 0}),
				new LQtMagias(2 ,new uint[]{2, 0, 0, 0, 0, 0, 0, 0, 0}),
				new LQtMagias(3 ,new uint[]{2, 1, 0, 0, 0, 0, 0, 0, 0}),
				new LQtMagias(4 ,new uint[]{2, 2, 0, 0, 0, 0, 0, 0, 0}),
				new LQtMagias(5 ,new uint[]{2, 2, 1, 0, 0, 0, 0, 0, 0}),
				new LQtMagias(6 ,new uint[]{3, 2, 2, 0, 0, 0, 0, 0, 0}),
				new LQtMagias(7 ,new uint[]{3, 2, 2, 1, 0, 0, 0, 0, 0}),
				new LQtMagias(8 ,new uint[]{3, 3, 2, 2, 0, 0, 0, 0, 0}),
				new LQtMagias(9 ,new uint[]{3, 3, 2, 2, 1, 0, 0, 0, 0}),
				new LQtMagias(10,new uint[]{3, 3, 3, 2, 2, 0, 0, 0, 0}),
				new LQtMagias(11,new uint[]{4, 3, 3, 2, 2, 1, 0, 0, 0}),
				new LQtMagias(12,new uint[]{4, 3, 3, 3, 2, 2, 0, 0, 0}),
				new LQtMagias(13,new uint[]{4, 4, 3, 3, 2, 2, 1, 0, 0}),
				new LQtMagias(14,new uint[]{4, 4, 3, 3, 3, 2, 2, 0, 0}),
				new LQtMagias(15,new uint[]{5, 4, 4, 3, 3, 2, 2, 1, 0}),
				new LQtMagias(16,new uint[]{5, 4, 4, 3, 3, 3, 2, 2, 0}),
				new LQtMagias(17,new uint[]{5, 5, 4, 4, 3, 3, 2, 2, 1}),
				new LQtMagias(18,new uint[]{5, 5, 4, 4, 3, 3, 3, 2, 2}),
				new LQtMagias(19,new uint[]{5, 5, 5, 4, 4, 3, 3, 2, 2}),
				new LQtMagias(20,new uint[]{6, 6, 5, 4, 4, 3, 3, 3, 2}),
			};
            int n = -3;
		    int A = -4;
		    int d = -5;
			var CorpoAfastarDadosvidaMortoVivo = new LAfastarDadosvidaMortoVivo[] {
				new LAfastarDadosvidaMortoVivo(1 ,new int[]{ 1, 13, 17, 19,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(2 ,new int[]{ 2, 11, 15, 18, 20,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(3 ,new int[]{ 3,  9, 13, 17, 19,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(4 ,new int[]{ 4,  7, 11, 15, 18, 20,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(5 ,new int[]{ 5,  5,  9, 13, 17, 19,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(6 ,new int[]{ 6,  3,  7, 11, 15, 18, 20,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(7 ,new int[]{ 7,  A,  5,  9, 13, 17, 19,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(8 ,new int[]{ 8,  A,  3,  7, 11, 15, 18, 20,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(9 ,new int[]{ 9,  A,  2,  5,  9, 13, 17, 19,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(10,new int[]{10,  d,  A,  3,  7, 11, 15, 18, 20,  n,  n,  n,  n,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(11,new int[]{11,  d,  A,  2,  5,  9, 13, 17, 19, 20,  n,  n,  n,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(12,new int[]{12,  d,  A,  A,  3,  7, 11, 15, 18, 19, 20,  n,  n,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(13,new int[]{13,  d,  d,  A,  2,  5,  9, 13, 17, 18, 19, 20,  n,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(14,new int[]{14,  d,  d,  A,  A,  3,  7, 11, 15, 17, 18, 19, 20,  n,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(15,new int[]{15,  d,  d,  d,  A,  2,  5,  9, 13, 15, 17, 18, 19, 20,  n,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(16,new int[]{16,  d,  d,  d,  A,  A,  3,  7, 11, 13, 15, 17, 18, 19, 20,  n,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(17,new int[]{17,  d,  d,  d,  d,  A,  2,  5,  9, 11, 13, 15, 17, 18, 19, 20,  n,  n,  n}),
				new LAfastarDadosvidaMortoVivo(18,new int[]{18,  d,  d,  d,  d,  A,  A,  3,  7,  9, 11, 13, 15, 17, 18, 19, 20,  n,  n}),
				new LAfastarDadosvidaMortoVivo(19,new int[]{19,  d,  d,  d,  d,  d,  A,  2,  5,  7,  9, 11, 13, 15, 17, 18, 19, 20,  n}),
				new LAfastarDadosvidaMortoVivo(20,new int[]{20,  d,  d,  d,  d,  d,  A,  A,  3,  5,  7,  9, 11, 13, 15, 17, 18, 19, 20}),
			};
			var CorpoTalentosLadrao = new LTalentosLadrao[] {
					new LTalentosLadrao( 1, (15,new Rolagem(D8   )), (10,new Rolagem(D8   )), 80, 20, 20, 2,2),
					new LTalentosLadrao( 2, (20,new Rolagem(D8   )), (15,new Rolagem(D8   )), 81, 25, 25, 2,2),
					new LTalentosLadrao( 3, (25,new Rolagem(D8   )), (20,new Rolagem(D8   )), 82, 30, 30, 2,2),
					new LTalentosLadrao( 4, (30,new Rolagem(D8   )), (25,new Rolagem(D8   )), 83, 35, 35, 2,2),
					new LTalentosLadrao( 5, (35,new Rolagem(D8   )), (30,new Rolagem(D8   )), 84, 40, 40, 3,2),
					new LTalentosLadrao( 6, (40,new Rolagem(D6   )), (35,new Rolagem(D6   )), 85, 45, 45, 3,3),
					new LTalentosLadrao( 7, (45,new Rolagem(D6   )), (40,new Rolagem(D6   )), 86, 50, 50, 3,3),
					new LTalentosLadrao( 8, (50,new Rolagem(D6   )), (45,new Rolagem(D6   )), 87, 55, 55, 3,3),
					new LTalentosLadrao( 9, (55,new Rolagem(D6   )), (50,new Rolagem(D6   )), 88, 60, 60, 3,3),
					new LTalentosLadrao(10, (60,new Rolagem(D6   )), (55,new Rolagem(D6   )), 89, 65, 65, 4,3),
					new LTalentosLadrao(11, (62,new Rolagem(D4   )), (60,new Rolagem(D4   )), 90, 70, 70, 4,3),
					new LTalentosLadrao(12, (64,new Rolagem(D4   )), (62,new Rolagem(D4   )), 91, 72, 72, 4,4),
					new LTalentosLadrao(13, (66,new Rolagem(D4   )), (64,new Rolagem(D4   )), 92, 74, 74, 4,4),
					new LTalentosLadrao(14, (68,new Rolagem(D4   )), (66,new Rolagem(D4   )), 93, 76, 76, 4,4),
					new LTalentosLadrao(15, (70,new Rolagem(D4   )), (68,new Rolagem(D4   )), 94, 78, 78, 4,4),
					new LTalentosLadrao(16, (72,new Rolagem(0,0,1)), (70,new Rolagem(0,0,1)), 95, 80, 80, 5,4),
					new LTalentosLadrao(17, (74,new Rolagem(0,0,1)), (72,new Rolagem(0,0,1)), 96, 82, 82, 5,4),
					new LTalentosLadrao(18, (76,new Rolagem(0,0,1)), (74,new Rolagem(0,0,1)), 97, 84, 84, 5,5),
					new LTalentosLadrao(19, (78,new Rolagem(0,0,1)), (76,new Rolagem(0,0,1)), 98, 86, 86, 5,5),
					new LTalentosLadrao(20, (80,new Rolagem(0,0,1)), (78,new Rolagem(0,0,1)), 99, 88, 88, 5,5),
				};
			var CorpoCapacidadeCarga = new LCapacidadeCarga[]{
				new LCapacidadeCarga(new(0 ,3)  , new(10 , 20  , 30) ),
				new LCapacidadeCarga(new(4 ,8)  , new(20 , 30  , 50) ),
				new LCapacidadeCarga(new(9 ,12) , new(30 , 50  , 70) ),
				new LCapacidadeCarga(new(13,15) , new(40 , 70  , 90) ),
				new LCapacidadeCarga(new(16,17) , new(50 , 80  , 100)),
				new LCapacidadeCarga(new(18,20) , new(60 , 100 , 120)),
				new LCapacidadeCarga(new(21,22) , new(90 , 130 , 150))
			}; 
			TabelaAtributos = new Tabela<uint,MinMax, LTabelaAtributos>( new ConfigChaveIntervalo<LTabelaAtributos>(), CorpoAtributos);
			
			HomemDeArmas 	= new Tabela<uint, LTabelaClasse>(CorpoHomemDeArmas);
			Ladino 			= new Tabela<uint, LTabelaClasse>(CorpoLadino);
			Clerigo			= new Tabela<uint, LTabelaClasse>(CorpoClerigo);
			Mago 			= new Tabela<uint, LTabelaClasse>(CorpoMago);
			MagiasClerigo	= new Tabela<uint, LQtMagias>(CorpoMagiasClerigo);
			MagiasMago 		= new Tabela<uint, LQtMagias>(CorpoMagiasMago);
			AfastarDadosvidaMortoVivo = new Tabela<uint,LAfastarDadosvidaMortoVivo>(CorpoAfastarDadosvidaMortoVivo);
			TalentosLadrao	= new Tabela<uint,LTalentosLadrao >(CorpoTalentosLadrao);
			TabelaCapacidadeCarga	= new Tabela<uint,MinMax,LCapacidadeCarga>(new ConfigChaveIntervalo<LCapacidadeCarga>(),CorpoCapacidadeCarga);
		}
	}	
}
