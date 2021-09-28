using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Xml.Schema;
using Microsoft.Win32;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Execute(object sender, RoutedEventArgs e)
        {

            switch (ComboBox.Text)
            {
                case ("text -> vigenere"):
                {
                    textbox2.Text = vigenere(textbox1.Text, true, textbox3.Text);
                    break;
                }
                case ("vigenere -> text"):
                {
                    textbox2.Text = vigenere(textbox1.Text, false, textbox3.Text);
                    break;
                }
                case ("text -> cesar"):
                {
                    textbox2.Text = cesar(textbox1.Text, false, textbox3.Text);
                    break;
                }
                case ("cesar -> text"):
                {
                    textbox2.Text = cesar(textbox1.Text, true, textbox3.Text);
                    break;
                }

                case ("text -> binaire"):
                {
                    textbox2.Text = StringToBinary(textbox1.Text);
                    break;
                }
                case ("binaire -> text"):
                {
                    textbox2.Text = BinaryToString(textbox1.Text);
                    break;
                }
                case ("text -> hexa"):
                {
                    textbox2.Text = StringToHexa(textbox1.Text);
                    break;
                }
                case ("hexa -> text"):
                {
                    textbox2.Text = HexaToString(textbox1.Text);
                    break;
                }
                default:
                {
                    textbox2.Text = BinaryToString(textbox1.Text);
                    break;

                }


            }

        }

        private void import(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                textbox1.Text = File.ReadAllText(openFileDialog.FileName);

        }
        private void export(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if(saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, textbox2.Text);

        }

        public static string StringToHexa(string text)
        {
            StringBuilder sH = new StringBuilder();
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            foreach (char c in text.ToCharArray())
            {
                sH.Append(Convert.ToByte(c).ToString("x2"));
                sH.Append(" ");
            }

            return sH.ToString();
        }

        public static string HexaToString(string text)
        {
            List<Byte> byteList = new List<Byte>();
            string[] s = text.Split(' ');


            try
            {

                foreach (string s1 in s)
                {
                    byteList.Add(Convert.ToByte(s1, 16));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return Encoding.ASCII.GetString(byteList.ToArray());
        }

        private static int Mod(int a, int b)
        {
            return (a % b + b) % b;
        }

        public static String vigenere(string str, bool way, String cles)
        {
            int specount = 0;
            String output = "";
            bool verif = false;
            for (int i = 0; i < cles.Length; ++i)
                if (!char.IsLetter(cles[i]))
                    verif = true;
            if (verif)
            {
                MessageBox.Show("Erreur dans la cles", "My App", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;

            }

            for (int i = 0; i < str.Length; ++i)
            {
                if (char.IsLetter(str[i]))
                {
                    bool cIsUpper = char.IsUpper(str[i]);
                    char offset = cIsUpper ? 'A' : 'a';
                    int keyIndex = (i - specount) % (cles.Length);
                    int k = (cIsUpper ? char.ToUpper(cles[keyIndex]) : char.ToLower(cles[keyIndex])) - offset;
                    k = way ? k : -k;
                    char ch = (char) ((Mod(((str[i] + k) - offset), 26)) + offset);
                    output += ch;
                }
                else
                {
                    specount++;
                    output += str[i];
                }
            }

            return output;

        }

        public static String cesar(string str, bool way, string cle)
        {
            String output = "";

            try
            {
                int cles = int.Parse(cle);
                cles = cles > 26 ? cles -= 26 : cles;
                for (int i = 0; i < str.Length; ++i)
                {
                    if (char.IsLetter(str[i]))
                    {
                        bool cIsUpper = char.IsUpper(str[i]);
                        char offset = cIsUpper ? 'A' : 'a';
                        int k = (cIsUpper ? char.ToUpper('A') : char.ToLower('A')) - offset - cles;
                        k = way ? k : -k;
                        char ch = (char) ((Mod(((str[i] + k) - offset), 26)) + offset);
                        output += ch;
                    }
                    else
                    {
                        output += str[i];
                    }
                }

                return output;
            }
            catch
            {

                MessageBox.Show("Erreur dans la cles", "My App", MessageBoxButton.OK, MessageBoxImage.Warning);

            }

            return null;


        }

        public static string StringToBinary(string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
                sb.Append(" ");
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();
            string[] s = data.Split(' ');
            try
            {
                foreach (string s1 in s)
                {
                    byteList.Add(Convert.ToByte(s1, 2));
                }
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