using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldDragon
{
    namespace Classes
    {
        interface IClasse
        {
            string Nome { get; }
            Rolagem DadoVida { get; }
            uint Nivel(uint Experiencia);
            uint JogadaProtecao(uint Nivel);
            uint BaseAtaque(uint Nivel);
            uint QtDadoVida(uint Nivel);
        }
        class Mago : IClasse
        {
            public string Nome => "Mago";
            public Rolagem DadoVida => new Rolagem(4);
            public uint Nivel(uint Nivel) => 1;
            public uint JogadaProtecao(uint Nivel) => 1;
            public uint BaseAtaque(uint Nivel) => 2;
            public uint QtDadoVida(uint Nivel) => 2;

        }
    }
}
