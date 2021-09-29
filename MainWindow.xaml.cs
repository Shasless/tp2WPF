using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using Microsoft.Win32;

namespace WpfApplication1
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void codedecode()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                switch (ComboBox.Text)
                {
                    case "text -> vigenere":
                    {
                        textbox2.Text = vigenere(textbox1.Text, true, textbox3.Text);
                        break;
                    }
                    case "vigenere -> text":
                    {
                        textbox2.Text = vigenere(textbox1.Text, false, textbox3.Text);
                        break;
                    }
                    case "text -> cesar":
                    {
                        textbox2.Text = cesar(textbox1.Text, false, textbox3.Text);
                        break;
                    }
                    case "cesar -> text":
                    {
                        textbox2.Text = cesar(textbox1.Text, true, textbox3.Text);
                        break;
                    }

                    case "text -> binaire":
                    {
                        textbox2.Text = StringToBinary(textbox1.Text);
                        break;
                    }
                    case "binaire -> text":
                    {
                        textbox2.Text = BinaryToString(textbox1.Text);
                        break;
                    }
                    case "text -> hexa":
                    {
                        textbox2.Text = StringToHexa(textbox1.Text);
                        break;
                    }
                    case "hexa -> text":
                    {
                        textbox2.Text = HexaToString(textbox1.Text);
                        break;
                    }
                    default:
                    {
                        MessageBox.Show("Erreur, ce n'est pas censé arriver", "Erreur message", MessageBoxButton.OK, MessageBoxImage.Warning);

                        break;
                    }
                }
            }));
        }

        private void Execute(object sender, RoutedEventArgs e)
        {
            var t = new Thread(codedecode);


            t.Start();
        }

        private void import(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                textbox1.Text = File.ReadAllText(openFileDialog.FileName);
        }

        private void export(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, textbox2.Text);
        }

        private static string StringToHexa(string text)
        {
            var sH = new StringBuilder();
            foreach (var c in text)
            {
                sH.Append(Convert.ToByte(c).ToString("x2"));
                sH.Append(" ");
            }

            return sH.ToString();
        }

        private static string HexaToString(string text)
        {
            var byteList = new List<byte>();
            var s = text.Split(' ');


            try
            {
                foreach (var s1 in s) byteList.Add(Convert.ToByte(s1, 16));
            }
            catch
            {
                //ignore
            }


            return Encoding.ASCII.GetString(byteList.ToArray());
        }

        private static int Mod(int a, int b)
        {
            return (a % b + b) % b;
        }

        private static string vigenere(string str, bool way, string cles)
        {
            var specount = 0;
            var output = "";
            var verif = false;
            for (var i = 0; i < cles.Length; ++i)
                if (!char.IsLetter(cles[i]))
                    verif = true;
            if (verif)
            {
                MessageBox.Show("Erreur dans la cles", "Erreur messsage", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            for (var i = 0; i < str.Length; ++i)
                if (char.IsLetter(str[i]))
                {
                    var cIsUpper = char.IsUpper(str[i]);
                    var offset = cIsUpper ? 'A' : 'a';
                    var keyIndex = (i - specount) % cles.Length;
                    var k = (cIsUpper ? char.ToUpper(cles[keyIndex]) : char.ToLower(cles[keyIndex])) - offset;
                    k = way ? k : -k;
                    var ch = (char) (Mod(str[i] + k - offset, 26) + offset);
                    output += ch;
                }
                else
                {
                    specount++;
                    output += str[i];
                }

            return output;
        }

        private static string cesar(string str, bool way, string cle)
        {
            var output = "";

            try
            {
                var cles = int.Parse(cle);
                for (var i = 0; i < str.Length; ++i)
                    if (char.IsLetter(str[i]))
                    {
                        var cIsUpper = char.IsUpper(str[i]);
                        var offset = cIsUpper ? 'A' : 'a';
                        var k = (cIsUpper ? char.ToUpper('A') : char.ToLower('A')) - offset - cles;
                        k = way ? k : -k;
                        var ch = (char) (Mod(str[i] + k - offset, 26) + offset);
                        output += ch;
                    }
                    else
                    {
                        output += str[i];
                    }

                return output;
            }
            catch
            {
                MessageBox.Show("Erreur dans la cles", "Erreur message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return null;
        }

        private static string StringToBinary(string data)
        {
            var sb = new StringBuilder();

            foreach (var c in data)
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
                sb.Append(" ");
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private static string BinaryToString(string data)
        {
            var byteList = new List<byte>();
            var s = data.Split(' ');
            try
            {
                foreach (var s1 in s) byteList.Add(Convert.ToByte(s1, 2));
            }
            catch
            {
                MessageBox.Show("Erreur dans le formatage du binaire", "My App", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            return Encoding.ASCII.GetString(byteList.ToArray());
        }
    }
}