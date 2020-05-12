using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MassVMTGenerator
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        String folderPath = "";

        public MainWindow()
        {
            InitializeComponent();
            SurfacePropSelectBox.SelectedIndex = 0;
            ShaderSelectBox.SelectedIndex = 0;
            initDatafromJson(); // Insert the data from the .json files in the ComboBox and ListBox
        }

        //--------------------
        //-- INIT FUNCTIONS --
        //--------------------

        public void initDatafromJson()
        {
            // Insert data in the SurfaceProp combobox
            try
            {
                var listSurfaceProp = JsonConvert.DeserializeObject<Dictionary<String, String>>(File.ReadAllText("parameters/surfaceprop.json"));
                foreach (KeyValuePair<String, String> item in listSurfaceProp)
                {
                    ComboBoxItem cbi = new ComboBoxItem(); cbi.Content = item.Value;
                    SurfacePropSelectBox.Items.Add(cbi);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            // Insert data in the Shader combobox
            try
            {
                var listShader = JsonConvert.DeserializeObject<Dictionary<String, String>>(File.ReadAllText("parameters/shader.json"));
                foreach (KeyValuePair<String, String> item in listShader)
                {
                    ComboBoxItem cbi = new ComboBoxItem(); cbi.Content = item.Value;
                    ShaderSelectBox.Items.Add(cbi);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Insert in Parameter ListBox
            try
            {
                var listParams = JsonConvert.DeserializeObject<Dictionary<String, String>>(File.ReadAllText("parameters/basic_parameters.json"));
                foreach (KeyValuePair<String, String> item in listParams)
                {
                    TextBlock tbk = new TextBlock(); tbk.Text = item.Value;
                    CheckBox cbx = new CheckBox(); cbx.Content = tbk; Parameters_CheckBox.Children.Add(cbx);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //-----------------------
        //-- GENERIC FUNCTIONS --
        //-----------------------

        
        private bool isFolderExist(string dirname)
        {
            return System.IO.Directory.Exists(dirname);
        }

        private bool isFolderFullPath(string dirname)
        {
            return System.IO.Path.IsPathRooted(dirname);
        }
        
        private string OpenFolder()
        {
            OpenFileDialog folderDialog = new OpenFileDialog();
            folderDialog.CheckFileExists = false;
            folderDialog.FileName = "[Folder Selection]"; // default name
            Nullable<bool> result = folderDialog.ShowDialog();

            if (result == true)
            {
                string savePath = System.IO.Path.GetDirectoryName(folderDialog.FileName);
                return savePath;
            }

            return "";
        }

        //---------------
        //-- VMT MAKER --
        //---------------

        private String getFolderPath()
        {
            return this.folderPath + "\\";
        }
        private String getBaseTexture()
        {
            String basetexture = BaseTexture_TextBox.Text;

            if (basetexture != "") // A basetexture path was provided
            {
                basetexture = basetexture.Replace("\"", string.Empty).Replace("'", string.Empty); // Remove " and '

                if (!basetexture.EndsWith("/") && !basetexture.EndsWith("\\")) // Add a / at the end of the path even if the user forgets it
                {
                    basetexture += "/";
                }
            }
            return basetexture;
        }

        private String getShader()
        {
            String shader = "";
            if (ShaderSelectBox.SelectedIndex > 0)
            {
                shader = (String)((ComboBoxItem)ShaderSelectBox.SelectedValue).Content;
            }
            return shader;
        }

        private List<String> getvtfsList()
        {
            List<String> vtfsList = new List<String>();
            foreach (ListBoxItem item in Vtf_ListBox.SelectedItems)
            {
                vtfsList.Add(System.IO.Path.GetFileNameWithoutExtension((String)item.Content));
            }
            return vtfsList;
        }

        private List<KeyValuePair<string, string>> getMainParam()
        {
            List<KeyValuePair<string, string>> mainParam = new List<KeyValuePair<string, string>>();

            String surfaceProp = "";
            if (SurfacePropSelectBox.SelectedIndex > 0)
            {
                surfaceProp = (String)((ComboBoxItem)SurfacePropSelectBox.SelectedValue).Content;
                mainParam.Add(new KeyValuePair<string, string>("$surfaceprop", surfaceProp));
            }

            try
            {
                foreach (CheckBox cbx in Parameters_CheckBox.Children)
                {
                    if ((bool)cbx.IsChecked)
                    {
                        //mainParam.Add(new KeyValuePair<string, string>((string)cbx.Content, "1"));
                        mainParam.Add(new KeyValuePair<string, string>(((TextBlock)cbx.Content).Text, "1"));
                    }
                }
            }
            catch { }
            return mainParam;

        }

        // Return a VMTGenerator class
        private VMTGenerator getDataForm()
        {
            String folderPath = getFolderPath();
            String basetexture = getBaseTexture();
            String shader = getShader();     
            List<String> vtfsList = getvtfsList();
            List<KeyValuePair<string, string>> mainParam = getMainParam();
            return new VMTGenerator(folderPath, shader, basetexture, vtfsList, mainParam);
        }

        //--------------------
        //-- EVENTS HANDLER --
        //--------------------

        private void Check_BrowseSelectAll(bool state)
        {
            Browse_SelectAll.IsChecked = state;
        }

        private void Enable_BrowseSelectAll(bool state)
        {
            Browse_SelectAll.IsEnabled = state;
        }

        private void Visibility_Generate_Preview_Buttons()
        {
            if (Vtf_ListBox.Items.Count > 0) // Atleast 1 vtf loaded
            {
                Enable_BrowseSelectAll(true);
                GenerateButton.Visibility = Visibility.Visible;
                PreviewButton.Visibility = Visibility.Visible;
            }
            else
            {
                GenerateButton.Visibility = Visibility.Hidden;
                PreviewButton.Visibility = Visibility.Hidden;
            }
        }

        private void Toggle_Generate_Preview_Buttons()
        {
            if (Vtf_ListBox.SelectedItems.Count > 0) // Atleast 1 vtf selected
            {
                GenerateButton.IsEnabled = true;
                PreviewButton.IsEnabled = true;
            }
            else
            {
                GenerateButton.IsEnabled = false;
                PreviewButton.IsEnabled = false;
            }
        }
        
        private void Vtf_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            Toggle_Generate_Preview_Buttons();
        }


        // SelectAll Button

        private void Browse_SelectAll_Checked(object sender, RoutedEventArgs e)
        {
            bool bl = (bool)Browse_SelectAll.IsChecked;
            if (bl)
            {
                foreach (ListBoxItem lbi in Vtf_ListBox.Items)
                {
                    lbi.IsSelected = true;
                    lbi.Focus();
                }
            }
            else
            {
                foreach (ListBoxItem lbi in Vtf_ListBox.Items)
                {
                    lbi.IsSelected = false;
                }
            }
        }
        
        // Button Browse

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // folderPath is a variable of the class
            folderPath = OpenFolder();
            Vtf_ListBox.Items.Clear();
            Check_BrowseSelectAll(false); // Uncheck the select all button
            Enable_BrowseSelectAll(false); // Disable the select all button
            Visibility_Generate_Preview_Buttons();
            Toggle_Generate_Preview_Buttons();
            if (folderPath != "")
            {
                if (isFolderExist(folderPath))
                {
                    if (isFolderFullPath(folderPath))
                    {
                        try
                        {
                            String[] listVtfs = Directory.GetFiles(folderPath, "*.vtf");
                            foreach (String fileVtf in listVtfs)
                            {
                                ListBoxItem lbi = new ListBoxItem();
                                lbi.Content = System.IO.Path.GetFileName(fileVtf);
                                Vtf_ListBox.Items.Add(lbi);
                            }
                            Visibility_Generate_Preview_Buttons();
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine(exc.Message);
                        }
                    }
                }
            }
        }



        // Button Preview

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            VMTGenerator vmtClass = getDataForm();
            if (Vtf_ListBox.Items.Count > 0) // Atleast 1 .vtf loaded
            {
                int idx = Vtf_ListBox.SelectedIndex;
                if (idx != -1) // One vtf is selected
                {
                    Preview_Box.Text = vmtClass.getVMTString((String)((ListBoxItem)Vtf_ListBox.Items.GetItemAt(idx)).Content);
                }
                else
                {
                    Preview_Box.Text = vmtClass.getVMTString((String)((ListBoxItem)Vtf_ListBox.Items.GetItemAt(0)).Content);
                }
            }
        }

        // Button Generate

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            VMTGenerator vmtClass = getDataForm();
            vmtClass.CreateVMTs();
            Log_Box.Text = vmtClass.getLogCreation();
        }

        

        
    }
}
