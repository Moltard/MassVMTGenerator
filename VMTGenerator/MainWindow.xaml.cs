using Newtonsoft.Json;
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
using VMTGenerator.Tool;
using VMTGenerator.Tool.Utils;

namespace VMTGenerator
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Store the path to the VTF files
        /// </summary>
        private static string OutputPath = string.Empty;

        //--------------------------------------
        //------------- Init GUI ---------------
        //--------------------------------------

        public MainWindow()
        {
            InitializeComponent();
            InitJsonDirectory();
            InitShader();
            InitSurfaceProp();
            InitParameters();
        }

        //--------------------------------------
        //---------- Init JSON Folder ----------
        //--------------------------------------

        /// <summary>
        /// Verify if the "Parameters/" directory exists and create it if needed
        /// </summary>
        private void InitJsonDirectory()
        {
            JsonUtils.CreateJsonDirectory();
        }

        //--------------------------------------
        //------------ Init Shader -------------
        //--------------------------------------

        /// <summary>
        /// Read shader.json and store the data in the Shader ComboBox
        /// </summary>
        private void InitShader()
        {
            ShaderSelectBox.SelectedIndex = 0;

            string jsonText = JsonUtils.ReadJson(JsonFileEnum.Shader);
            if (jsonText != null)
            {
                List<string> valueList = JsonUtils.DeserializeArray(jsonText);

                foreach (string value in valueList)
                {
                    ComboBoxItem cbi = new ComboBoxItem
                    {
                        Content = value
                    };
                    ShaderSelectBox.Items.Add(cbi);
                }
            }
        }

        //--------------------------------------
        //---------- Init SurfaceProp ----------
        //--------------------------------------

        /// <summary>
        /// Read surfaceprop.json and store the data in the SurfaceProp ComboBox
        /// </summary>
        private void InitSurfaceProp()
        {
            SurfacePropSelectBox.SelectedIndex = 0;
            string jsonText = JsonUtils.ReadJson(JsonFileEnum.SurfaceProp);
            if (jsonText != null)
            {
                List<string> valueList = JsonUtils.DeserializeArray(jsonText);

                foreach (string value in valueList)
                {
                    ComboBoxItem cbi = new ComboBoxItem
                    {
                        Content = value
                    };
                    SurfacePropSelectBox.Items.Add(cbi);
                }
            }
        }

        //--------------------------------------
        //---------- Init Parameters -----------
        //--------------------------------------

        /// <summary>
        /// Read parameters.json and store the data in the Parameter list
        /// </summary>
        private void InitParameters()
        {
            string jsonText = JsonUtils.ReadJson(JsonFileEnum.Parameters);
            if (jsonText != null)
            {
                List<string> valueList = JsonUtils.DeserializeArray(jsonText);

                foreach (string value in valueList)
                {
                    TextBlock tbk = new TextBlock
                    {
                        Text = value
                    };
                    CheckBox cbx = new CheckBox
                    {
                        Content = tbk
                    };
                    Parameters_CheckBox.Children.Add(cbx);
                }
            }
        }

        //--------------------------------------
        //------------- VMT Data ---------------
        //--------------------------------------

        /// <summary>
        /// Get the selected shader
        /// </summary>
        /// <returns></returns>
        private string GetShader()
        {
            string shader = string.Empty;
            if (ShaderSelectBox.SelectedIndex > 0)
            {
                shader = (string)((ComboBoxItem)ShaderSelectBox.SelectedValue).Content;
            }
            return shader;
        }

        /// <summary>
        /// Get the given basetexture path
        /// </summary>
        /// <returns></returns>
        private string GetBaseTexture()
        {
            string baseTexture = BaseTexture_TextBox.Text.Trim(); 
            if (baseTexture != string.Empty)
            {
                baseTexture = baseTexture.Replace("\"", string.Empty)
                    .Replace("'", string.Empty);
                // If the user doesn't add a / at the end, add it
                if (!baseTexture.EndsWith("/") && !baseTexture.EndsWith("\\")) 
                {
                    baseTexture += "/";
                }
            }
            return baseTexture;
        }

        /// <summary>
        /// Get the list of all selected VTF files
        /// </summary>
        /// <returns></returns>
        private List<string> GetVTFsList()
        {
            List<string> vtfsList = new List<string>();
            foreach (ListBoxItem item in VTF_ListBox.SelectedItems)
            {
                string vtfFile = (string)item.Content;
                vtfsList.Add(System.IO.Path.GetFileNameWithoutExtension(vtfFile));
            }
            return vtfsList;
        }

        /// <summary>
        /// Get the list of checked parameters
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> GetParameters()
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            
            if (SurfacePropSelectBox.SelectedIndex > 0)
            {
                string surfaceProp = (string)((ComboBoxItem)SurfacePropSelectBox.SelectedValue).Content;
                parameters.Add(new KeyValuePair<string, string>(VMTUtils.KeySurfaceProp, surfaceProp));
            }

            try
            {
                foreach (CheckBox cbx in Parameters_CheckBox.Children)
                {
                    if ((bool)cbx.IsChecked)
                    {
                        string keyParameter = ((TextBlock)cbx.Content).Text;
                        parameters.Add(new KeyValuePair<string, string>(keyParameter, VMTUtils.ValueParameter));
                    }
                }
            }
            catch { }
            return parameters;

        }


        /// <summary>
        /// Store the selected options into a <see cref="VMTProperties"/> class
        /// </summary>
        /// <returns></returns>
        private VMTProperties GetVMTData()
        {
            VMTProperties vmt = new VMTProperties
            {
                Shader = GetShader(),
                BaseTexture = GetBaseTexture(),
                ParametersList = GetParameters()

            };
            return vmt;
        }


        //--------------------------------------
        //------------ UI Handlers -------------
        //--------------------------------------

        /// <summary>
        /// Check the 'Select All' checkbox
        /// </summary>
        private void BrowseSelectAll_Check()
        {
            Browse_SelectAll.IsChecked = true;
        }

        /// <summary>
        /// Uncheck the 'Select All' checkbox
        /// </summary>
        private void BrowseSelectAll_Uncheck()
        {
            Browse_SelectAll.IsChecked = false;
        }

        /// <summary>
        /// Display the 'Select All' checkbox
        /// </summary>
        private void BrowseSelectAll_Display()
        {
            Browse_SelectAll.IsEnabled = true;
        }

        /// <summary>
        /// Hide the 'Select All' checkbox
        /// </summary>
        private void BrowseSelectAll_Hide()
        {
            Browse_SelectAll.IsEnabled = false;
        }

        /// <summary>
        /// Display the Preview and Generate buttons
        /// </summary>
        private void PreviewGenerateButtons_Display()
        {
            GenerateButton.Visibility = Visibility.Visible;
            PreviewButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hide the Preview and Generate buttons
        /// </summary>
        private void PreviewGenerateButtons_Hide()
        {
            GenerateButton.Visibility = Visibility.Hidden;
            PreviewButton.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Disable the Preview and Generate buttons
        /// </summary>
        private void PreviewGenerateButtons_Disable()
        {
            GenerateButton.IsEnabled = false;
            PreviewButton.IsEnabled = false;
        }

        /// <summary>
        /// Enable the Preview and Generate buttons
        /// </summary>
        private void PreviewGenerateButtons_Enable()
        {
            GenerateButton.IsEnabled = true;
            PreviewButton.IsEnabled = true;
        }


        /// <summary>
        /// Make Visible or Hide the Preview and Generate buttons depending if at least 1 VTF is selected
        /// </summary>
        private void UpdateVisibility_Buttons()
        {
            if (VTF_ListBox.Items.Count > 0) // At least 1 VTF loaded
            {
                BrowseSelectAll_Display();
                PreviewGenerateButtons_Display();
            }
            else
            {
                BrowseSelectAll_Hide();
                PreviewGenerateButtons_Hide();
            }
        }

        /// <summary>
        /// Enable or Disable the Preview and Generate buttons depending if at least 1 VTF is selected
        /// </summary>
        private void UpdateUI_Buttons()
        {
            if (VTF_ListBox.SelectedItems.Count > 0) // At least 1 VTF selected
            {
                PreviewGenerateButtons_Enable();
            }
            else
            {
                PreviewGenerateButtons_Disable();
            }
        }

        //--------------------------------------
        //------------ Drag & Drop -------------
        //--------------------------------------


        /// <summary>
        /// Get the list of drag and dropped files
        /// </summary>
        /// <param name="e"></param>
        /// <returns>Returns the list of drag and drop files or null if invalid</returns>
        private string[] DragDropGetPaths(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                return e.Data.GetData(DataFormats.FileDrop, true) as string[];
            }
            return null;
        }

        /// <summary>
        /// Get the name of the directory drag and dropped.
        /// Or the name of the directory of the drag and dropped file
        /// </summary>
        /// <param name="droppedFilePaths"></param>
        /// <returns></returns>
        private string GetDroppedDirectoryName(string[] droppedFilePaths)
        {
            string directory = null;
            if (droppedFilePaths != null)
            {
                foreach (var path in droppedFilePaths)
                {
                    // Get the directory path of the dragged files
                    if (System.IO.Directory.Exists(path))
                    {
                        directory = path;
                        break;
                    }
                    else if (System.IO.File.Exists(path))
                    {
                        directory = FilesUtils.GetDirectoryName(path);
                        break;
                    }
                }
            }
            return directory;
        }

        //--------------------------------------
        //------------- VTFs List --------------
        //--------------------------------------

        /// <summary>
        /// Load the VTF files in the given directory and add them to the list
        /// </summary>
        /// <param name="directory"></param>
        private void BrowseDirectory(string directory)
        {
            if (directory != null)
            {
                VTF_ListBox.Items.Clear();
                BrowseSelectAll_Uncheck();
                BrowseSelectAll_Hide();
                PreviewGenerateButtons_Disable();
                PreviewGenerateButtons_Hide();
                if (System.IO.Directory.Exists(directory))
                {
                    if (System.IO.Path.IsPathRooted(directory))
                    {
                        string[] vtfsList = FilesUtils.GetFiles(directory, VMTUtils.ExtensionsVTF);
                        if (vtfsList != null)
                        {
                            foreach (string vtfFile in vtfsList)
                            {
                                ListBoxItem lbi = new ListBoxItem
                                {
                                    Content = System.IO.Path.GetFileName(vtfFile)
                                };
                                VTF_ListBox.Items.Add(lbi);
                            }
                            OutputPath = directory;
                            UpdateVisibility_Buttons();
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Event Handler for the Browse button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string directory = FilesUtils.OpenFolderDialog();
            BrowseDirectory(directory);
        }

        /// <summary>
        /// Event Handler for dropping a folder in the VTF Listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VTF_ListBox_Drop(object sender, DragEventArgs e)
        {
            string directory = GetDroppedDirectoryName(DragDropGetPaths(e));
            BrowseDirectory(directory);
        }

        /// <summary>
        /// Event Handler for the VTF Selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VTF_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUI_Buttons();
        }
        
        /// <summary>
        /// Event Handler for the Select All Checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browse_SelectAll_Checked(object sender, RoutedEventArgs e)
        {
            bool isChecked = (bool)Browse_SelectAll.IsChecked;
            if (isChecked)
            {
                foreach (ListBoxItem lbi in VTF_ListBox.Items)
                {
                    lbi.IsSelected = true;
                    lbi.Focus();
                }
            }
            else
            {
                foreach (ListBoxItem lbi in VTF_ListBox.Items)
                {
                    lbi.IsSelected = false;
                }
            }
        }

        //--------------------------------------
        //--------- Preview & Generate ---------
        //--------------------------------------

        /// <summary>
        /// Event Handler for the Preview Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (VTF_ListBox.Items.Count > 0) // Atleast 1 VTF loaded
            {
                VMTProperties vmt = GetVMTData();
                int index = VTF_ListBox.SelectedIndex;
                if (index == -1) // No VTF selected
                {
                    index = 0;
                }
                string vtfFile = (string)((ListBoxItem)VTF_ListBox.Items.GetItemAt(index)).Content;
                string vtfName = System.IO.Path.GetFileNameWithoutExtension(vtfFile);
                Preview_Box.Text = VMTUtils.GetVMTString(vmt, vtfName);
            }
        }

        /// <summary>
        /// Event Handler for the Generate button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            VMTGeneratorData vmtGenerator = new VMTGeneratorData(OutputPath, GetVTFsList(), GetVMTData());
            vmtGenerator.GenerateVMTs();
            Logs_Box.Text = vmtGenerator.Logs;
        }

        
    }
}
