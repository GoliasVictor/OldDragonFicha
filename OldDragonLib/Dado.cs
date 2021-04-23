using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldDragon
{ 
    public class Dado
    {
        protected static Random Rnd = new Random();
        public uint QtFace;
        public virtual uint Rolar() => (uint)Rnd.Next((int)QtFace + 1);
        public virtual IEnumerable<uint> Rolar(uint Qt)
        {
            for (int i = 0; i < Qt; i++)
                yield return Rolar();
        }
        public static int Rolar(int Dado, int Qt = 1, int Modificador = 0)
        {
            int Resultado = 0;
            for (int i = 0; i < Qt; i++)
                Resultado += Rnd.Next(Dado + 1);
            return Resultado + Modificador;
        }

        public Dado(uint qtFace) => QtFace = qtFace;
        

        public static readonly Dado D2 = new Dado(2);
        public static readonly Dado D3 = new Dado(3);
        public static readonly Dado D4 = new Dado(4);
        public static readonly Dado D6 = new Dado(6);
        public static readonly Dado D8 = new Dado(8);
        public static readonly Dado D10 = new Dado(10);
        public static readonly Dado D12 = new Dado(12);
        public static readonly Dado D20 = new Dado(20);
    }
    public class Rolagem
    {
        public Dado Dado;
        public uint QtRolagens;
        public int Modificador;
        public int Rolar()
        {
            int Resultado = 0;
            for (int i = 0; i < QtRolagens; i++)
                Resultado += (int)Dado.Rolar() + 1;
            return Resultado + Modificador;
        }
        public virtual IEnumerable<int> Rolar(uint Qt)
        {
            for (int i = 0; i < Qt; i++)
                yield return Rolar();
        }
        public override string ToString() => $"{QtRolagens}D{Dado.QtFace}" + (Modificador != 0 ? $"+{Modificador}" : ""); 
        public Rolagem(uint qtFace, uint qtRolagens = 1, int modificador = 0)
        {
            Dado = new Dado(qtFace);
            QtRolagens = qtRolagens;
            Modificador = modificador;
        }
        public static implicit operator Rolagem(Dado D) => new Rolagem(D);
        public Rolagem(Dado dado, uint qtRolagens = 1, int modificador = 0) : this(dado.QtFace, qtRolagens, modificador) {; }
    }

}
