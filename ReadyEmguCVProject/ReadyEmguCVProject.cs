using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;                 
using Emgu.CV.Structure;      
using Emgu.Util;               
using Emgu.CV.CvEnum;
namespace ReadyEmguCVProject
{
    public partial class ReadyEmguCVProject : Form
    {



        public ReadyEmguCVProject()
        {
            InitializeComponent(); //inicjalizacja komponentów
        }


        private HaarCascade haar; //detektor obiektów
        private Image image; 
      
        
        private void btnStart_Click(object sender, EventArgs e)
        {
           
            try
            {
                Image<Bgr, byte> ImageFrame = new Image<Bgr, byte>(new Bitmap(image)); //pobranie obrazka

                String text;
                text = listBox1.GetItemText(listBox1.SelectedItem); //pobranie wartości z listy rozwijanej (wartości 1.1; 1.2; 1.3; 1.4)
                double parameter;
                try
                {
                    parameter = double.Parse(text.Replace(".", ",")); 
                }
                catch (Exception)
                {
                    parameter = 1.1; //wartość domyślna - najbardziej dokładne wykrywanie, lecz najwolniejsze
                }
                
                if (ImageFrame != null)   //sprawdzenie czy obrazek został załadowany
                {
                    // konwersja obrazka zgodna z ideą algorytmu Violi-Jonesa
                    Image<Gray, byte> grayframe = ImageFrame.Convert<Gray, byte>();

                    //wykrywanie twarzy
                    var faces = grayframe.DetectHaarCascade(haar, parameter, 4,
                                            HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                            new Size(25, 25))[0];

                    int TotalFaces = faces.Length;   // zmienna przechowująca ilość pobranych twarzy

                    MessageBox.Show("Wykryto: " + TotalFaces.ToString() + " twarzy");
                    //zaznaczenie każdej znalezionej twarzy zieloną ramką
                    foreach (var face in faces)
                    {
                        ImageFrame.Draw(face.rect, new Bgr(Color.Green), 3);
                    }


                }
                
                pictureBox1.Image = ImageFrame.ToBitmap();   //wyswietlenie obrazka
                this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; //wyskalowanie obrazka

            }
            catch (Exception)
            {
                MessageBox.Show("Najpierw załaduj obrazek");
            }

        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

       

        private void ReadyEmguCVProject_Load(object sender, EventArgs e)
        {
            haar = new HaarCascade("haarcascade_frontalface_alt_tree.xml"); //załadowanie bazy twarzy
        }

       

        private void button1_Click(object sender, EventArgs e)
        {


            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Wybierz plik";
                dlg.Filter = "jpg files (*.jpg)|*.jpg";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    image = new Bitmap(dlg.FileName); //załadowanie wybranego obrazka
                    pictureBox1.Image = image;
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }


        }

    }
}
