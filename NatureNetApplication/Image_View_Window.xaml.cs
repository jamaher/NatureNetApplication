﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlServerCe;
using System.Collections;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Surface.Presentation.Controls;

namespace NatureNetApplication
{
    /// <summary>
    /// Interaction logic for Image_View_Window.xaml
    /// </summary>
    public partial class Image_View_Window : UserControl
    {
        private string item;
        int local_setter;
        private ObservableCollection<String> names;
        public ObservableCollection<String> Names
        {
            get
            {
                if (names == null)
                {
                    names = new ObservableCollection<String>();
                }

                return names;
            }
        }

        
        public Image_View_Window()
        {
            
            InitializeComponent();
           // Image_View_Window.DataContextProperty = this;
            Images_LibraryItems.ItemsSource = Names;
            //Images_LibraryItems.ItemsSource = 
        }

        public Image_View_Window(string item)
        {
            InitializeComponent();
            Databox.DataContext = this;
          //  Images_LibraryItems.ItemsSource = Names;
            DragDrop.AddDropHandler(this, oncursordrop);
             this.item = item;
            label1.Content = item;
            // TODO: Complete member initialization
            List<string> photos = new List<string>();
            SqlCeConnection conn = null;
             string query = "SELECT Image_Database.Image_location FROM Image_Map_to_Tags INNER JOIN Image_Database ON Image_Map_to_Tags.Image_tag_name = Image_Database.Image_name AND Image_Map_to_Tags.image_tag LIKE '"+item+"'";
            string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
            string connectionString = string.Format("Data Source=" + filesPath);
            conn = new SqlCeConnection(connectionString);
            conn.Open();
            SqlCeCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            SqlCeDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (File.Exists(reader.GetString(0)))
                {
                    if (!Names.Contains(reader.GetString(0)))
                    {
                        Names.Add(reader.GetString(0));
                    }
                }
            }
            conn.Close();
            //ObservableCollection<string> items = new ObservableCollection<string>();
            //foreach (string s in Course)
            //{
            //    items.Add(s);
             Images_LibraryItems.DataContext = this;
            //}
            foreach (string s in Names)
            {
                Images_LibraryItems.Items.Add(s);
            }
           
           // Images_LibraryItems.ItemsSource = Names;
            if (Names.Count == 0)
            {
                surfaceButton5.Visibility = Visibility.Visible;
            }
            //using (SqlConnection cn = new SqlConnection(connectionString))
            //{
            //    using (SqlCommand cm = new SqlCommand(query, cn))
            //    {
            //        cn.Open();
            //        SqlDataReader reader = cm.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            Course.Add(reader.GetString(0));
            //        }
            //    }
            //}
           
            
        }
        //public Image_View_Window(string item,int setter)
        //{
        //    InitializeComponent();
        //    Images_LibraryItems.ItemsSource = Names;
        //    surfaceButton5.Visibility = Visibility.Visible;
        //    this.item = item;
        //    label1.Content = item;
        //    // TODO: Complete member initialization
        //    List<string> photos = new List<string>();
        //    SqlCeConnection conn = null;
        //    string query = "SELECT Image_Database.Image_location FROM Image_Map_to_Tags INNER JOIN Image_Database ON Image_Map_to_Tags.Image_tag_name = Image_Database.Image_name AND Image_Map_to_Tags.image_tag LIKE '" + item + "'";
        //    string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
        //    string connectionString = string.Format("Data Source=" + filesPath);
        //    conn = new SqlCeConnection(connectionString);
        //    conn.Open();
        //    SqlCeCommand cmd = conn.CreateCommand();
        //    cmd.CommandText = query;
        //    SqlCeDataReader reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        if (File.Exists(reader.GetString(0)))
        //        {
        //            if (!Names.Contains(reader.GetString(0)))
        //            {
        //                Names.Add(reader.GetString(0));
        //            }
        //        }
        //    }
        //    conn.Close();
        //    //ObservableCollection<string> items = new ObservableCollection<string>();
        //    //foreach (string s in Course)
        //    //{
        //    //    items.Add(s);

        //    //}
        //    foreach (string s in Names)
        //    {

        //    }
        //    Images_LibraryItems.DataContext = this;
        //    Images_LibraryItems.ItemsSource = Names;
        //    //using (SqlConnection cn = new SqlConnection(connectionString))
        //    //{
        //    //    using (SqlCommand cm = new SqlCommand(query, cn))
        //    //    {
        //    //        cn.Open();
        //    //        SqlDataReader reader = cm.ExecuteReader();
        //    //        while (reader.Read())
        //    //        {
        //    //            Course.Add(reader.GetString(0));
        //    //        }
        //    //    }
        //    //}


        //}
        private void surfaceButton1_Click(object sender, RoutedEventArgs e)
        {
            surfaceButton1.Tag = "Enabled";
            surfaceButton1.Background = Brushes.Red;
            surfaceButton2.Background = Brushes.Gray;
            surfaceButton2.Tag = "Disabled";
            ContributionBox.Visibility = System.Windows.Visibility.Visible;
            surfaceButton3.Visibility = System.Windows.Visibility.Visible;
            ContributionBox.Text = "Please enter Bio-Diversity-Data";
            surfaceButton1.BorderThickness = new Thickness( 10);
            surfaceButton1.BorderBrush = Brushes.Black;
            surfaceButton2.BorderBrush = null;
            Databox.Items.Clear();
            SqlCeConnection conn = null;

            string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
            string connectionString = string.Format("Data Source=" + filesPath);
            conn = new SqlCeConnection(connectionString);
            SqlCeCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = "SELECT Ideas, Tag_name FROM Data_associated_tags WHERE (Tag_name = N'"+item+"')";

            SqlCeDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Databox.Items.Add(reader.GetString(0));
            }
            conn.Close();

        }

        private void surfaceButton2_Click(object sender, RoutedEventArgs e)
        {
            surfaceButton2.Background = Brushes.Red;
            surfaceButton1.Background = Brushes.Gray;
            surfaceButton2.Tag = "Enabled";
            surfaceButton1.Tag = "Disabled";
            surfaceButton2.BorderThickness = new Thickness(10);
            surfaceButton2.BorderBrush = Brushes.Black;
            surfaceButton1.BorderBrush = null;
            ContributionBox.Visibility = System.Windows.Visibility.Visible;
            ContributionBox.Text = "Please enter Design-Ideas";
            surfaceButton3.Visibility = System.Windows.Visibility.Visible;
            Databox.Items.Clear();
                  SqlCeConnection conn = null;

            string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
            string connectionString = string.Format("Data Source=" + filesPath);
            conn = new SqlCeConnection(connectionString);
            SqlCeCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = "SELECT Idea_content FROM Ideas";
            
            SqlCeDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Databox.Items.Add(reader.GetString(0));
            }
            conn.Close();
            
        }

        private void surfaceButton3_Click(object sender, RoutedEventArgs e)
        {
            string datacontext="";
            if ((string)(surfaceButton1.Tag.ToString()) == "Enabled")
            {
               datacontext =  "Bio";

            }
            else
            {
                if ((string)(surfaceButton2.Tag.ToString()) == "Enabled")
                {
                    datacontext = "Design";
                }
 
            }

            if (datacontext == "Bio")
            {
                if (ContributionBox.Text.ToString() == "Please enter Bio-Diversity-Data" || ContributionBox.Text.ToString() == "The data has been saved")
                {
                    ContributionBox.Text = "Please enter Bio-Diversity-Data";
                }
                else
                {
                    SqlCeConnection conn = null;

                    string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
                    string connectionString = string.Format("Data Source=" + filesPath);
                    conn = new SqlCeConnection(connectionString);
                    SqlCeCommand cmd = conn.CreateCommand();
                    conn.Open();
                    cmd.CommandText = "INSERT INTO Data_associated_tags (Ideas, Tag_name) VALUES ('" + ContributionBox.Text.ToString() + "','" + label1.Content.ToString() + "')";
                    Databox.Items.Add(ContributionBox.Text.ToString());
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    ContributionBox.Text = "The data has been saved";

                }
            }
            else
                if (ContributionBox.Text.ToString() == "Please enter Design-Ideas" || ContributionBox.Text.ToString() == "The data has been saved")
                {
                    ContributionBox.Text = "Please enter Design-Ideas";
                }
                else
                {
                    if (datacontext == "Design")
                    {
                        SqlCeConnection conn = null;

                        string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
                        string connectionString = string.Format("Data Source=" + filesPath);
                        conn = new SqlCeConnection(connectionString);
                        SqlCeCommand cmd = conn.CreateCommand();
                        conn.Open();
                        cmd.CommandText = "INSERT INTO Ideas (Idea_content) VALUES ('" + ContributionBox.Text.ToString() + "')";
                        Databox.Items.Add(ContributionBox.Text.ToString());
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        ContributionBox.Text = "The data has been saved";
                       
                    }
                }

            
        }

        private void surfaceButton4_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            ScatterViewItem svi = null;
            while (parent as ScatterView == null)
            {
                if (parent is ScatterViewItem)
                    svi = parent as ScatterViewItem;
                parent = VisualTreeHelper.GetParent(parent);
            }

            ((ScatterView)parent).Items.Remove(svi);
        }

        private void Images_LibraryItems_DragCompleted(object sender, Microsoft.Surface.Presentation.SurfaceDragCompletedEventArgs e)
        {

        }

        private void Images_LibraryItems_StackSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Images_LibraryItems.ItemsSource = Names;
        }

        private void surfaceButton5_Click(object sender, RoutedEventArgs e)
        {
            SqlCeConnection conn = null;
                        string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
            string connectionString = string.Format("Data Source=" + filesPath);
            conn = new SqlCeConnection(connectionString);
            SqlCeCommand cmd = conn.CreateCommand();
            if (Names.Count == 0)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(this);
                ScatterViewItem svi = null;
                while (parent as ScatterView == null)
                {
                    if (parent is ScatterViewItem)
                        svi = parent as ScatterViewItem;
                    parent = VisualTreeHelper.GetParent(parent);
                }

                ((ScatterView)parent).Items.Remove(svi);

            }
            else
            {
                foreach (string s in Names)
                {
                    conn.Open();
                   string result= System.IO.Path.GetFileName(s);
                   cmd.CommandText = "INSERT INTO Image_Map_to_Tags (Image_tag_name, image_tag) VALUES ('" + result + "', '"+label1.Content.ToString()+"')";
                   cmd.ExecuteScalar();
                   conn.Close();
                   Databox.Items.Clear();
                   Databox.Items.Add("Photos saved");
                }
                
 
            }


        }

        public void oncursordrop(object sender, DragEventArgs args)
        { 

        }
    }
}
