using static OldDragon.Tabelas;
namespace OldDragon
{
	public partial class Personagem
	{
		public VlCarga VlCapacidadeCarga	=> TabelaCapacidadeCarga[Atributos.For].Carga;
		public int Arrombar 				=> TabelaAtributos[Atributos.Des].Arrombar;
		public int Armadilhas 				=> TabelaAtributos[Atributos.Des].Armadilhas;
		public int Furtividade 				=> TabelaAtributos[Atributos.Des].FurtividadEPungar;
		public int Pungar 					=> TabelaAtributos[Atributos.Des].FurtividadEPungar;
		public int ChanceRessureicao 		=> TabelaAtributos[Atributos.Con].ChanceRessureicao;
		public int IdiomasAdicionais 		=> TabelaAtributos[Atributos.Int].IdiomasAdicionais;
		public int ChanceAprenderMagia		=> TabelaAtributos[Atributos.Int].ChanceAprenderMagia;
		public (uint C1, uint C2, uint C3) MagiasAdicionais => TabelaAtributos[Atributos.Sab].MagiasAdicionais;
		public int NumeroMaxSeguidores 		=> TabelaAtributos[Atributos.Sab].NumeroSeguidores;
		public int AjusteReacao 			=> TabelaAtributos[Atributos.Sab].AjusteReacao;
		public Rolagem QtMortoVivoAfastado 	=> TabelaAtributos[Atributos.Sab].QtMortoVivoAfastado;
	}
}