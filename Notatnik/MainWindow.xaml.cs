using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace Notatnik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }


        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private int click = 0;

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if(this.WindowState== WindowState.Maximized)
            {
                click++;
            }
            click++;
            if (click == 1)
            {
                this.WindowState = WindowState.Maximized;
            }
            else if (click == 2)
            {
                this.WindowState = WindowState.Normal;
                click = 0;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string FileName = "Bez tytułu";
        private string path ="";
        private string DocumentText ="";

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            FileName= ofd.SafeFileName;
            path = ofd.FileName;
            title.Content = $"{FileName} - Notatnik";
            if (ofd.FileName != null)
            {
                StreamReader reader = new StreamReader(ofd.FileName);
                DocumentText = reader.ReadToEnd();
                Textarea.Text = DocumentText;
                reader.Close();
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
           Zapisz();
        }
        public void Zapisz()
        {
            if (path != "")
            {
                StreamWriter savefileWriter = new StreamWriter(path);

                savefileWriter.Write(Textarea.Text);
                DocumentText = Textarea.Text;
                savefileWriter.Close();
            }
            else 
            {
                SaveAs();
            }
            TitleManager();
        }
        public int CurrentLine
        {
            get { return Textarea.GetLineIndexFromCharacterIndex(Textarea.CaretIndex); }
        }
        public int CurrentColumn
        {
            get { return Textarea.CaretIndex - Textarea.GetCharacterIndexFromLineIndex(CurrentLine); }
        }
        private void Textarea_TextChanged(object sender, TextChangedEventArgs e)
        {
            ColumnCout.Text = CurrentLine.ToString();
            LineCount.Text = CurrentColumn.ToString();
            TitleManager();
        }
        private void SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "ALL *.*|(*.*) | TXT (.txt)|*.txt" ;
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                path = saveFileDialog.FileName;
                FileName = saveFileDialog.SafeFileName;
                StreamWriter sw = new StreamWriter(path);
                sw.Write(Textarea.Text);
                sw.Close();

            }
        }
        private void TitleManager()
        {
            if(Textarea.Text == DocumentText)
            {
                title.Content = FileName + " - " + "Notatnik";


            }
            else
            {
                title.Content = "*"+FileName + " - " + "Notatnik";

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TitleManager();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(Textarea.Text != DocumentText )
            {
                e.Cancel = true;
                if(MessageBox.Show("Nie masz zapisanego pliku. Chcesz zapisac?","Zapisz",MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Zapisz();
                    Environment.Exit(0);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e) 
        {
            SaveAs();
        }

        private void Newwindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);

            }
            catch(Exception ex)
            {
                MessageBox.Show("Nie moge tego zrobic" + ex.Message);
               
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (Textarea.Text != DocumentText)
            {
                if (MessageBox.Show("Nie masz zapisanego pliku. Chcesz zapisac?", "Zapisz", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Zapisz();
                    Title = "Bez tytułu";
                    path = "";
                    DocumentText = "";
                    Textarea.Text = "";
                    
                }
                else
                {
                    Title = "Bez tytułu";
                    path = "";
                    DocumentText = "";
                    Textarea.Text = "";

                }
            }
            else
            {
                Title = "Bez tytułu";
                path = "";
                DocumentText = "";
                Textarea.Text = "";
            }
        }

        private void Ustawieniastrony_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            pd.ShowDialog();

        }

        private void cofnij_Click(object sender, RoutedEventArgs e)
        {
            if(Textarea.CanUndo)
            {
                Textarea.Undo();
            }
        }

        private void Wytnij_Click(object sender, RoutedEventArgs e)
        {
            Textarea.Cut();
        }

        private void Kopiuj_Click(object sender, RoutedEventArgs e)
        {
            Textarea.Copy();
        }

        private void Wklej_Click(object sender, RoutedEventArgs e)
        {
            Textarea.Paste();
        }

        private void Usun_Click(object sender, RoutedEventArgs e)
        {
            Textarea.Text = Textarea.Text.Remove(Textarea.SelectionStart,Textarea.SelectionLength);
        }

        private void Znajdz_Click(object sender, RoutedEventArgs e)
        {

        }

        private void zaznacz_Click(object sender, RoutedEventArgs e)
        {
            Textarea.SelectAll();
        }

        private void godzina_Click(object sender, RoutedEventArgs e)
        {
            Textarea.Text = Textarea.Text.Insert(Textarea.CaretIndex, DateTime.Now.ToString());
        }
        private double zoomScale = 1;
        private double incremention = 0.1;
        ScaleTransform scaleTransform = new ScaleTransform(1,1);
        private void Przybliz_Click(object sender, RoutedEventArgs e)
        {
           
            scaleTransform.ScaleX += 0.1;
            scaleTransform.ScaleY += 0.1;
            Textarea.RenderTransform = scaleTransform;
        }

        private void Oddal_Click(object sender, RoutedEventArgs e)
        {
            scaleTransform.ScaleX -= 0.1;
            scaleTransform.ScaleY -= 0.1;
            Textarea.RenderTransform = scaleTransform;

           
        }

        private void basicLupa_Click(object sender, RoutedEventArgs e)
        {
            scaleTransform.ScaleX =1;
            scaleTransform.ScaleY =1;
        }

    

        private void Textarea_MouseMove(object sender, MouseEventArgs e)
        {
            ColumnCout.Text = CurrentLine.ToString();
            LineCount.Text = CurrentColumn.ToString();
        }
    }
}
