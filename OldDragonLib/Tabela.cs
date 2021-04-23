using System;
using System.Collections.Generic;
using System.Linq;

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
            VerificarLinha = MetodoProcura;
            this.Colunas = Colunas.ToList();
            if (!(Linhas is null))
                InserirVariasLinhas(Linhas);
            
        }
        public object Procurar(string Coluna, Predicate<Dictionary<string,object>> predicate)
        {
            var Res = from Linha in tabela
            where predicate(Linha)
            select Linha;
            return Res.First()[Coluna];
        }
        public void InserirLinha( params object[] Linha)
        {
            if (Linha.Length != Colunas.Count)
                throw new ArgumentOutOfRangeException();
            var LinhaNomeada = new Dictionary<string, object>();

            for (int i = 0; i < Linha.Length; i++)
                LinhaNomeada.Add(Colunas[i], Linha[i]);

            tabela.Add(LinhaNomeada);
        }
        public void InserirVariasLinhas( object[][] Linhas)
        {
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
    public class TabelaT<TKey, Linha>
    {
        public delegate bool MetodoProcuraTabela(TKey key, Linha Linha);
        readonly List<Linha> tabela = new List<Linha>();
        readonly MetodoProcuraTabela Procurar;
        public TabelaT(MetodoProcuraTabela MetodoProcura) => Procurar = MetodoProcura;
        public void InserirLinha(Linha Linha) => tabela.Add(Linha);
        public void InserirVariasLinhas(params Linha[]Linhas) => tabela.AddRange(Linhas);
        
        public Linha this[TKey key]
        {
            get {
                
                foreach (var Linha in tabela)
                    if (Procurar(key, Linha))
                        return Linha;
                throw new KeyNotFoundException("Linha não encontrada");
            }
        }
    }
 


}
