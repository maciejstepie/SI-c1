using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

/*
||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
|||||||||||||||||||||| Maciej Stępień       ||||||||||||||||||||||
|||||||||||||||||||||| UWM, ISI 3           ||||||||||||||||||||||
|||||||||||||||||||||| 24.03.2018,          ||||||||||||||||||||||
|||||||||||||||||||||| Sztuczna Inteligencja||||||||||||||||||||||
|||||||||||||||||||||| "Operacje na         ||||||||||||||||||||||
||||||||||||||||||||||   systemach danych"  ||||||||||||||||||||||
||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
*/



namespace DaneZPlikuOkienko
{
    public partial class DaneZPliku : Form
    {
        private string[][] atrType;
        private string[][] wczytaneDane;

        //************************************************************
        //Globalne zmienne 

        //Przechowuje inforamcje czy sa jakiekolwiek numeryczne dane
        bool saNumeryczne = false;
        bool isData = false;
        //Przechowuje Informacje o tym ktore indeksy atrybutów sa numeryczne oraz symboliczne
        List<Numerics> ListNumerics;
        List<int> IndeksSym;
        List<List<string>> ListaList;
        
        public struct Numerics
        {
            public int Numer;
            public double Min;
            public double Max;

            public Numerics (int num, double min, double max)
            {
                this.Numer = num;
                this.Min = min;
                this.Max = max;


            }
        }

        
        //******************************************************
        
        public DaneZPliku()
        {
            InitializeComponent();
        }

        private void btnWybierzPlik_Click(object sender, EventArgs e)
        {
            DialogResult resultWyboruPliku = ofd.ShowDialog(); // wybieramy plik
            if (resultWyboruPliku != DialogResult.OK)
                return;

            tbSciezkaDoPlikuSystemu.Text = ofd.FileName;
            //A jak duży plik??
            FileInfo f1 = new FileInfo(ofd.FileName);
            tbSize.Text = "Plik: " + f1.Length / 1024 + "KB";
            if(f1.Length > 100 * 1024)
            {

                MessageBox.Show("Za duży plik, system nie ogarnie!");
                return ;
            }
            
            //
            
            string trescPliku = System.IO.File.ReadAllText(ofd.FileName); // wczytujemy treść pliku do zmiennej
            string[] wiersze = trescPliku.Trim().Split(new char[] { '\n' }); // treść pliku dzielimy wg znaku końca linii, dzięki czemu otrzymamy każdy wiersz w oddzielnej komórce tablicy
            wczytaneDane = new string[wiersze.Length][];   // Tworzymy zmienną, która będzie przechowywała wczytane dane. Tablica będzie miała tyle wierszy ile wierszy było z wczytanego poliku

            for (int i = 0; i < wiersze.Length; i++)
            {
                string wiersz = wiersze[i].Trim();     // przypisuję i-ty element tablicy do zmiennej wiersz
                string[] cyfry = wiersz.Split(new char[] { ' ' });   // dzielimy wiersz po znaku spacji, dzięki czemu otrzymamy tablicę cyfry, w której każda oddzielna komórka to czyfra z wiersza
                wczytaneDane[i] = new string[cyfry.Length];    // Do tablicy w której będą dane finalne dokładamy wiersz w postaci tablicy integerów tak długą jak długa jest tablica cyfry, czyli tyle ile było cyfr w jednym wierszu
                for (int j = 0; j < cyfry.Length; j++)
                {
                    string cyfra = cyfry[j].Trim(); // przypisuję j-tą cyfrę do zmiennej cyfra
                    wczytaneDane[i][j] = cyfra;  
                }
            }

            tbResult.Text = TablicaDoString(wczytaneDane);
        }

        private void btnWybierzPlikZTypami_Click(object sender, EventArgs e)
        {
            DialogResult resultWyboruPliku = ofd.ShowDialog(); // wybieramy plik
            if (resultWyboruPliku != DialogResult.OK)
                return;

            tbSciezkaDoPlikuZTypami.Text = ofd.FileName;
            string trescPliku = System.IO.File.ReadAllText(ofd.FileName).Trim();

            string[] wiersze = trescPliku.Split(new char[] { '\n' });
            atrType = new string[wiersze.Length][];
            for (int i = 0; i < wiersze.Length; i++)
            {
                string wiersz = wiersze[i].Trim();
                atrType[i] = wiersz.Split(new char[] { ' ' });
            }

            tbAtrType.Text = TablicaDoString(atrType);
        }

        public string TablicaDoString<T>(T[][] tab)
        {
            string result = "";
            for (int i = 0; i < tab.Length; i++)
            {
                for (int j = 0; j < tab[i].Length; j++)
                {
                    result += tab[i][j].ToString() + " ";
                }
                result = result.Trim() + Environment.NewLine;
            }

            return result;
        }


        public double StringToDouble(string liczba)
        {
            double result; liczba = liczba.Trim();
            if (!double.TryParse(liczba.Replace(',', '.'), out result) && !double.TryParse(liczba.Replace('.', ','), out result))
                throw new Exception("Nie udało się skonwertować liczby do double");

            return result;
        }


        public int StringToInt(string liczba)
        {
            int result;
            if (!int.TryParse(liczba.Trim(), out result))
                throw new Exception("Nie udało się skonwertować liczby do int");

            return result;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //Zapisuje liczbe wierszy z wczytanych danych na potrzeby innych funkcji
            int Rows = 0;
            //Gdyby sie okazalo ze nie ma wierszy
            try
            {
                Rows = wczytaneDane.GetLength(0);
            }
            catch { }


            // Tablica z wczytanymi danymi dostępna poniżej
            // this.wczytaneDane;

            // Tablica z typami atrybutów
            // this.atrType;

            // 
            // Przykład konwersji string to double 
            string sLiczbaDouble = "1.5";
            double dsLiczbaDouble = StringToDouble(sLiczbaDouble);

            // Przykład konwersji string to int 
            string sLiczbaInt = "1";
            int iLiczbaInt = StringToInt(sLiczbaInt);


            //A)
            //A1) Wypisac rodzaje decyzji i ich ilosci
            //A2) Jezeli sa atrybuty numeryczne znalesc najmniejsze i najwieksze

            //B)

            //B1)- dla kazdego atrybutu wypisujemy liczbe róznych dostepnych wartosci,
            //B2)- dla kazdego atrybutu wypisujemy liste wszystkich róznych dostepnych wartosci,
            //B3)- odchylenie standardowe dla poszczególnych atrybutów w całym systemie i w klasach
            //decyzyjnych (dotyczy atrybutów numerycznych).
            

            //Czyszczenie przed nastepnym sprawdzeniem
            saNumeryczne = false;
            ListNumerics = new List<Numerics>();
            IndeksSym = new List<int>();
            tb1.Text = "";
            tb2.Text = "";
            isData = QuickCheck(); //Sprawdzenie czy sa jakiekolwiek dane w oknie #1
            tbMinMax.Text = "";
            ListaList = new List<List<string>>();
            tbOd.Text = "";
            richTextBox1.Text = "";

            // Tu się wykonuję kod A1,2:

            try
            {
              saNumeryczne =  CheckIsNumeric(); //Sprawdzenie czy sa numeryczne dane w oknie #2
            }
           
            catch(Exception ee)
            {
                MessageBox.Show("Coś nie tak robisz chyba! \n" + ee.Message ,"Bład!",MessageBoxButtons.OK ,MessageBoxIcon.Error);
            }
            // Tu się wykonuję kod A2:
            if (saNumeryczne)CheckMinMax(Rows);
            // Tu się wykonuję kod B1,2:
            if (isData)AtributsLists(Rows);
            // Tu się wykonuję kod B3:
            if (saNumeryczne) Odchylenie(Rows);
            
        }

        bool QuickCheck()
        {
            if (wczytaneDane != null) return true;
            else
                return false;
        }
  
        //Funkcja sprawdza z jakimi danymi mami doczynienia oraz jezeli natrafi na numeryczne zapamietuje to
        bool CheckIsNumeric()
        {
            bool tmp = false;

            for (int i = 0; i < atrType.GetLength(0); i++)
            {

                if (atrType[i][1] == "n")
                {
                    tmp = true;
                    ListNumerics.Add(new Numerics(i,99999,-99999));
                }
                if (atrType[i][1] == "s")
                {                    
                    IndeksSym.Add(i);
                }

            }

            tb1.Text = "Symboliczne: " + IndeksSym.Count;
            tb1.Text += Environment.NewLine + "Numeryczne: " + ListNumerics.Count;

            return tmp;
        }


        //Funckja sprawdzajaca min i max numerycznych argumentow
        void CheckMinMax(int Rows)
        { 
            for (int i = 0; i < ListNumerics.Count;i++)
            {
                //Potrzebne zmienne na potrzeby przeszukiwania
                double tmp = 0;

                //Testowo wypisuje indeksy ktore sa numeryczne
                tb2.Text += (ListNumerics[i].Numer + 1).ToString() + " ";
                //Zabezpieczenie przed pustym polem z danymi
                if (isData)
                {
                    tbMinMax.Text += "Dla atrybutu numer " + (ListNumerics[i].Numer + 1).ToString() + ": ";
                    for (int j = 0; j < Rows; j++)
                    {

                        tmp = StringToDouble(wczytaneDane[j][ListNumerics[i].Numer]);
                        if (tmp < ListNumerics[i].Min)
                        {
                            ListNumerics[i] = new Numerics(ListNumerics[i].Numer,tmp,ListNumerics[i].Max);
                        }
                        if (tmp > ListNumerics[i].Max)
                        {
                            ListNumerics[i] = new Numerics(ListNumerics[i].Numer, ListNumerics[i].Min, tmp);
                        }
                        //tbMinMax.Text += " " + wczytaneDane[j][ListNumerics[i].Numer];
                    }
                }

                tbMinMax.Text += "Mininum: \t" + ListNumerics[i].Min;
                tbMinMax.Text += " Maximum: " + ListNumerics[i].Max + Environment.NewLine;                          
            }

            if (!isData)
                MessageBox.Show("Brak danych do obróbki!");            
        }


        void AtributsLists(int Rows)
        {
            int wymiarY = Rows;
            int wymiarX = ListNumerics.Count + IndeksSym.Count;
            //tbAtributsCount.Text = wymiarX + " x " + wymiarY;
            
            for (int i  = 0; i < wymiarX ; i++)
            {
                //Tworzy nowa liste
                List<string> l1 = new List<string>();
                richTextBox1.Text += "Nr atrybutu: " + (i + 1).ToString() + ":";
                //Dodaje do niej swoje rzeczy
                for (int j = 0; j < wymiarY ; j++)
                {
                    //przeszukuje cyz jest juz taki string w liscie
                    var match = l1.FirstOrDefault(stringToCheck => stringToCheck.Contains(wczytaneDane[j][i]));

                    if (match == null)
                    {

                        richTextBox1.Text += " " + wczytaneDane[j][i];//i + " Y:" + j+ " ";//wczytaneDane[i][j] + "| ";
                        l1.Add(wczytaneDane[j][i]);
                    }
    //Do stuff
                }
                richTextBox1.Text += ", różnych wartości: " + l1.Count + Environment.NewLine + Environment.NewLine;
                //Dodaje liste do listy list
                ListaList.Add(l1);
            }

        }

        void Odchylenie(int Rows)
        {
            double srednia;
            double wariancja;
            
            foreach (var i in ListNumerics)
            {
                tbOd.Text += "Nr. Atr.: " + (i.Numer + 1) + "   \t";
                srednia = Round(CalcAverage(Rows, i.Numer));
                tbOd.Text += "Avr.: " + srednia;
                wariancja = Round(CalcVariance(Rows, i.Numer, srednia));
                tbOd.Text += "   \t" + "War.: " + wariancja;
                tbOd.Text += "   \t" + "Odch.: " + Round(Math.Sqrt(wariancja)) + Environment.NewLine;
            }
                        
        }

        //Oblicza srednia arytmetyczna
        double CalcAverage(int n, int numer)
        {
            double suma = 0;
            for (int j = 0; j < n; j++)
            {
                suma += StringToDouble(wczytaneDane[j][numer]);
                //wczytaneDane[j][numer];
            }

            return suma / n;
        }

        //Liczymy wariancje a w efekcie rowniez odchylenie
        double CalcVariance(int n,int numer, double Avr)
        {
            double tmp = 0;
            double sumaTmp = 0;
            if (n > 1)
            {
                for (int j = 0; j < n; j++)
                {
                    tmp = StringToDouble(wczytaneDane[j][numer]) - Avr;
                    sumaTmp += tmp * tmp;
                }
                return sumaTmp / n;
            }
            //jezeli brak danych lub jeden wiersz
            else                return 0;
                    }

        double Round(double a)
        {            
            return System.Math.Round(a, (int)numericUpDown1.Value);
        }
        /*

   // To get a Dictionary<string, int>
   var counts = rodzajeDecyzji.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
   string s = string.Join(" ,", counts.Select(x => x.Key + " = " + x.Value).ToArray());
   tb1.Text = s;//counts.Select(x => x.Key + "=" + x.Value).ToString();





*/

    }
}
