using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static OldDragon.Dado;
using OldDragon; 
using OldDragon.Itens;
using OldDragon.Atributo;
// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace OldDragonUwp
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        Personagem personagem
        {
            get;
            set;
        }

        public MainPage()
        {
            personagem = new Personagem("dougras", Raca.Racas["ElfoNegro"], Classes.Mago, 30000,Alinhamento.Caotico, atributos: new Atributos(10, 14, 11, 16, 9, 7));
            personagem.Iventario.AddRange(new List<Item>() {
                new Protecao("Armadura de placas", 13, 7, 3, -2, true,new Dinheiro(300,Moedas.PO)),
                new Protecao("Escudo madeira", 2, 1, Preco: new Dinheiro(3,Moedas.PO)),
                new Arma("Adaga", 1, D4, TamanhoArma.G, new HashSet<TipoArma>(){ TiposArma.Pe,TiposArma.Co}),
                new Arma("Espada", 1, D8, TamanhoArma.M, TiposArma.Co),
                new Arma("Flecha", 0.1, D6, TamanhoArma.P, TiposArma.Pe,Quantidade:20)
            });

            DataContext = personagem;

            this.InitializeComponent();

            NumBoxFor.Value = personagem.Atributos.For.Valor;
            NumBoxCon.Value = personagem.Atributos.Con.Valor;
            NumBoxDes.Value = personagem.Atributos.Des.Valor;
            NumBoxInt.Value = personagem.Atributos.Int.Valor;
            NumBoxSab.Value = personagem.Atributos.Sab.Valor;
            NumBoxCar.Value = personagem.Atributos.Car.Valor;
        }
        private void MudancaAtributo(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            var Atrs = new Atributos(
               (uint)NumBoxFor.Value,
               (uint)NumBoxCon.Value,
               (uint)NumBoxDes.Value,
               (uint)NumBoxInt.Value,
               (uint)NumBoxSab.Value,
               (uint)NumBoxCar.Value
           );
            personagem.Atributos = Atrs;
            TxBoxAjFor.Text = personagem.Atributos.For.Ajuste.ToString();
            TxBoxAjCon.Text = personagem.Atributos.Con.Ajuste.ToString();
            TxBoxAjDes.Text = personagem.Atributos.Des.Ajuste.ToString();
            TxBoxAjInt.Text = personagem.Atributos.Int.Ajuste.ToString();
            TxBoxAjSab.Text = personagem.Atributos.Sab.Ajuste.ToString();
            TxBoxAjCar.Text = personagem.Atributos.Car.Ajuste.ToString();
        }

    }
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? null : parameter == null ? value : string.Format((string)parameter, value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
        public static string FormatarTituloItem(Item item)
        {
            string Saida = "";
            if (item.Quantidade != 1)
                Saida += $"{item.Quantidade}x ";
            return Saida + $"{item.Nome} {item.Peso}Kg";
        }
    }
   
    public class SeletorItem : DataTemplateSelector
    {
        public DataTemplate Outros { get; set; }
        public DataTemplate Protecao { get; set; }
        public DataTemplate Arma { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            switch (item)
            {
                case Protecao _: return Protecao;
                case Arma _: return Arma;
                case Item _: return Outros;
                default:  return base.SelectTemplateCore(item, container);
            }
            
        }
    }
    
}
