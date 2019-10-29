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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
namespace Prctc
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
        List<User> us = new List<User>();
        List<User> newusers;
        //XmlSerializer formatter = new XmlSerializer(typeof(User));
        DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<User>));
        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            // User u = new User(textBox.Text, textBox1.Text);
            bool flag = false;
            using (FileStream fs = new FileStream("users.json", FileMode.OpenOrCreate))
            {
                //List<User> newusers = (List<User>)formatter.Deserialize(fs);
                newusers = (List<User>)jsonFormatter.ReadObject(fs);
                foreach (User un in newusers)
                {
                    if(un.username == textBox.Text && un.password == passwordBox.Password)
                    {
                        flag = true;
                        un.activated = true;
                        break;
                    }
                }
                if(flag==false)
                {
                    var res = MessageBox.Show("Error!","This user doesn't exist!",MessageBoxButton.OK,MessageBoxImage.Error);                }
            }
            if(flag==true)
            {
                using (FileStream fs = new FileStream("users.json", FileMode.Truncate))
                {
                    jsonFormatter.WriteObject(fs, newusers);
                }
                Diary a = new Diary();
                a.Show();
            }
                //string s = textBox.Text;
                //string s1 = textBox1.Text;
            }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           
            User u = new User(textBox.Text, passwordBox.Password);
            us.Add(u);
            using (FileStream fs = new FileStream("users.json", FileMode.Truncate))
            {
                jsonFormatter.WriteObject(fs,us);
                //formatter.Serialize(fs, u);
            }

            }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
           
            
                textBox1.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Hidden;
                textBox1.Text = passwordBox.Password;
            
           /* else
            {
                textBox1.Visibility = Visibility.Hidden;
                passwordBox.Visibility = Visibility.Visible;
                passwordBox.Focus();
            }*/
        }
        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox1.Visibility = Visibility.Hidden;
            passwordBox.Visibility = Visibility.Visible;
            passwordBox.Focus();
        }
    }
}
