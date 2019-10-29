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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.ComponentModel;
using Microsoft.Win32;
namespace Prctc
{
    /// <summary>
    /// Interaction logic for Diary.xaml
    /// </summary>
    public partial class Diary : Window
    {
        public Diary()
        {

            InitializeComponent();
            Loaded += Diary_Loaded;
            Closing +=OnWindowClosing;
        }
        User user =new User();
        List<User> newusers = new List<User>();
        int count = 0;
      DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<User>));
        DataContractJsonSerializer jsonFormatter1 = new DataContractJsonSerializer(typeof(string));
        private void Diary_Loaded(object sender, RoutedEventArgs e)
        {
            // do work here
            using (FileStream fs = new FileStream("users.json", FileMode.OpenOrCreate))
            {
                //List<User> newusers = (List<User>)formatter.Deserialize(fs);
                 newusers = (List<User>)jsonFormatter.ReadObject(fs);
                foreach (User un in newusers)
                {
                    if (un.activated == true)
                    {
                         user = un;
                        if(un.sheet[0]!=null)
                        {
                            textBox.Text = un.sheet[count];
                        }
                    }
                }
            }
            var res = MessageBox.Show("Don't forget to save page!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            using (FileStream fs = new FileStream("image.json", FileMode.OpenOrCreate))
            {
                string name1 = (string)jsonFormatter1.ReadObject(fs);
                
                ImageBrush b = new ImageBrush();
                b.ImageSource = new BitmapImage(new Uri(name1, UriKind.Relative));
                Background = b;
            }

               
        }
      
        private void button_Click(object sender, RoutedEventArgs e)
        {
            user.sheet.Add(textBox.Text);
            using (FileStream fs = new FileStream("users.json", FileMode.Truncate))
            {
                jsonFormatter.WriteObject(fs,newusers);

            }

            }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string s = "";
            user.sheet[count] = textBox.Text;
            count += 1;
            if (count<user.sheet.Count)
            { textBox.Text = user.sheet[count]; }
            else { user.sheet.Add(s);
                textBox.Text = s;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            user.sheet[count] = textBox.Text;
            if (count!=0)
            {
                count -= 1;
                textBox.Text = user.sheet[count];
                
            }
           
            else
            {
                var res = MessageBox.Show("It's first page!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (user.sheet.Count > 1)
            { user.sheet.Remove(user.sheet[count]);
                if (count == 0)
                {
                    count += 1;
                    textBox.Text = user.sheet[count];
                   
                }
                else
                {
                    count -= 1;
                    textBox.Text = user.sheet[count];
                  
                }
               
            }
            else
            {
                var res = MessageBox.Show("You would like to delete the last page. If you do that, then all diary will be deleted! Are you sure", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if(res==  MessageBoxResult.Yes)
                {
                    foreach (User un in newusers)
                    {
                        if(un==user)
                        {
                            newusers.Remove(un);
                            using (FileStream fs = new FileStream("users.json", FileMode.Truncate))
                            {
                                jsonFormatter.WriteObject(fs, newusers);
                            }
                        }
                    }
                    this.Close();
                    }
            }
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            user.activated = false;
            using (FileStream fs = new FileStream("users.json", FileMode.Truncate))
            {
                jsonFormatter.WriteObject(fs, newusers);
            }
            // Handle closing logic, set e.Cancel as needed
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ComboBox comboBox = (ComboBox)sender;
            TextBlock selectedItem=new TextBlock();
            /*if (comboBox.SelectedItem !=null)
            {
                 selectedItem = (ComboBoxItem)comboBox.SelectedItem;
                return;
            }*/
            selectedItem = (TextBlock)comboBox.SelectedItem;
            textBox.FontFamily = new FontFamily(selectedItem.Text);
            //selectedItem.ToString()
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            TextBlock selectedItem = new TextBlock();
            
            selectedItem = (TextBlock)comboBox1.SelectedItem;
           
            textBox.FontSize = Int32.Parse(selectedItem.Text);
            //textBox.Font
        }
        string name = "";
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                name = openFileDialog.FileName;
                
            }
            
            ImageBrush b = new ImageBrush();
            b.ImageSource = new BitmapImage(new Uri(name, UriKind.Relative));
            Background = b;
            using (FileStream fs = new FileStream("image.json", FileMode.OpenOrCreate))
            {
                jsonFormatter1.WriteObject(fs,name);
            }
        }
    }
}
