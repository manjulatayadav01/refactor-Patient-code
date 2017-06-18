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

using System.Xml;
using System.Xml.Linq;
using FeetImport.Repository;
using FeetImport.Bel;
using System.IO;
using System.Xml.Schema;

namespace FeetImport
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Xml documents (.xml)|*.xml";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            
            XDocument fleetXML = new XDocument();
            if (result == true)
            {
                // Open document
                // Get the selected file name and display in a TextBox
                string filename = dlg.FileName;
                FileNameTextBox.Text = filename;
                bool validationErrors = false;
                //Check for XML file exists
                if (File.Exists(filename))
                {
                    try
                    {
                        //Add XML Schema to validate XML file
                        string path = (CommonFunction.GetDirectoryName() + @"\XML\vehicle.xsd").Replace("\\", @"\");
                        XmlSchemaSet schema = new XmlSchemaSet();
                        schema.Add("", @"C:\Users\manju\documents\visual studio 2015\Projects\FeetImport\FeetImport\XML\vehicle.xsd");
                        fleetXML = XDocument.Load(filename);

                        fleetXML.Validate(schema, (s, ex) =>
                        {
                            ValidationError.Text = ex.Message;
                            validationErrors = true;
                        }
                        );
                        if (validationErrors)
                        {
                            ValidationFailed.Content = "Validation failed";
                        }


                    }
                    catch (FileNotFoundException ex)
                    {
                        ValidationError.Text = ex.Message;
                    }
                    catch (XmlException ex)
                    {
                        ValidationError.Text = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        ValidationError.Text = ex.Message;
                    }
                }

                //Get all vehicle type in Array
                string[] vehicleType = Enum.GetNames(typeof(VehicleType));

                //Assign Fleet fields from XML
                List<Fleet> fleet = new List<Fleet>();
                for (int i = 0; i< vehicleType.Length; i++)
                {
                    string name = vehicleType[i];
                    var fl = fleetXML.Descendants(name).Select(f =>

                     new Fleet
                     {
                         Year = Convert.ToInt32(f.Element("Year").Value),
                         Make = f.Element("Make").Value,
                         Model = f.Element("Model").Value,
                         VehicleName = name,
                         VINNumber = (f.Element("VinNumber") == null) ? "" : f.Element("VinNumber").Value,
                         LicensePlateNumber = (f.Element("LicensePlateNumber") == null) ? "" : f.Element("LicensePlateNumber").Value,
                         NauticalRegistrationNumber = (f.Element("NauticalRegistrationNumber") == null) ? "" : f.Element("NauticalRegistrationNumber").Value,
                     }                     
                     
                     ).ToList();

                    foreach (var g in fl)
                    {
                        fleet.Add(g);
                    }                  
                   
                }

                VehicleDataGrid.ItemsSource = fleet;               

            }
        }
    }
}
