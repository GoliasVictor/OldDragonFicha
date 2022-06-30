using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace OldDragon
{
	
	public class ConfiguracaoTabela<TSearchKey,TKey,TLinha>
		where TLinha : ILinhaTabela<TKey> 
	{
		
		public virtual bool CompararChaveLinha(TSearchKey key, TLinha Linha){
			return key.Equals(Linha.key);
		}		
		public virtual bool CompararLinhas(TLinha A, TLinha B){
			return A.key.Equals(B.key);
		}
	}
	public interface ILinhaTabela<T>
	{ 
	 	T key{get;}
	}
	public interface ILinhaTabela : ILinhaTabela<object>{ }
	public interface ITabela
	{
	}
	public class Tabela<TSearchKey,TKey,TLinha> : IEnumerable<TLinha> , ITabela 
		where TLinha : ILinhaTabela<TKey>
	{        

		readonly List<TLinha> tabela = new List<TLinha>();
		readonly ConfiguracaoTabela<TSearchKey,TKey,TLinha> Config;
		public Tabela(ConfiguracaoTabela<TSearchKey,TKey,TLinha> configuracaoTabela , params TLinha[] Linhas )
		{
			this.Config = configuracaoTabela ?? throw new ArgumentNullException(nameof(configuracaoTabela));
			if (Linhas is object)
				InserirVariasLinhas(Linhas); 
		} 
		public Tabela(params TLinha[] Linhas )
		{	
			Config = new ConfiguracaoTabela<TSearchKey,TKey,TLinha>();
			if (Linhas is object)
				InserirVariasLinhas(Linhas); 
		} 
		public TLinha Primeiro(Func<TLinha,bool> predicate)
		{
			if( predicate is null)
				throw new ArgumentNullException(nameof(predicate)); 
			return tabela.Where(predicate).FirstOrDefault();
		}
		public void InserirLinha( TLinha NovaLinha)
		{
			
			if (NovaLinha is null)
				throw new ArgumentNullException(nameof(NovaLinha)); 
			if(tabela.Any((Linha) => Config.CompararLinhas(Linha,NovaLinha)))
				throw new ArgumentException("Linha já existe");
			tabela.Add(NovaLinha);
		}
		public void InserirVariasLinhas( TLinha[] Linhas)
		{
			if (Linhas is null)
				throw new ArgumentNullException(nameof(Linhas)); 
			foreach (var Linha in Linhas) 
				InserirLinha(Linha);
		}

		public IEnumerator<TLinha> GetEnumerator()
		{
			return ((IEnumerable<TLinha>)tabela).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)tabela).GetEnumerator();
		}

		public TLinha this[TSearchKey key]
		{ 
			get
			{  
				foreach (var Linha in tabela)
					if (Config.CompararChaveLinha(key, Linha))
						return Linha;
				throw new KeyNotFoundException("Linha não encontrada"); 
			}
		}
	}
	public class Tabela<TSearchKey,TLinha> : Tabela<TSearchKey,TSearchKey,TLinha>  
		where TLinha : ILinhaTabela<TSearchKey>
	{
		public Tabela(ConfiguracaoTabela<TSearchKey,TSearchKey,TLinha> configuracaoTabela , params TLinha[] Linhas) 
			: base(configuracaoTabela,Linhas){} 
		public Tabela(params TLinha[] Linhas ):base(Linhas){}
	}
}
