using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace CalorieCalculator.API
{
    public class Calc
    {
        public static string DISTANCE_FROM_IDEAL_WEIGHT { get; set; }
        public static string IDEAL_WEIGHT { get; set; }
        public static string CALORIES { get; set; }


        public static void Calculate(string heightFeet, string heightInches, string weight, string age, The_sex sex)
        {
            //Clear old results
            DISTANCE_FROM_IDEAL_WEIGHT = "";
            IDEAL_WEIGHT = "";
            CALORIES = "";
            
            //Validate height (feet) is numeric value

            Patient patient = new Patient();
            patient.heightFeet = heightFeet;
            patient.heightInches = heightInches;
            patient.weight = weight;
            patient.age = age;
            patient.sex = sex;
            CalculateCalory(patient);

        }

        public static void Calculate(Patient patient)
        {
            //Clear old results
            DISTANCE_FROM_IDEAL_WEIGHT = "";
            IDEAL_WEIGHT = "";
            CALORIES = "";

            //Validate height (feet) is numeric value
            CalculateCalory(patient);
        }

        private static void CalculateCalory(Patient patient)
        {
            double feet = Convert.ToDouble(patient.heightFeet);
            double inch = Convert.ToDouble(patient.heightInches);
            double wgh = Convert.ToDouble(patient.weight);
            double personAge = Convert.ToDouble(patient.age);
            //Calculate calories
            CALORIES = CommonFunction.GetCaloryConsumption(feet, inch, wgh, personAge, patient.sex).ToString();
            //Calculate ideal weight
            IDEAL_WEIGHT = CommonFunction.GetIdealWeight(feet, inch, patient.sex).ToString();
            //Calculate and display distance from ideal weight
            DISTANCE_FROM_IDEAL_WEIGHT = (wgh - Convert.ToDouble(IDEAL_WEIGHT)).ToString();
        }

       

        public enum The_sex
        {
            Male,
            Female
        }

        public static void Save(string patientSsnPart1, string patientSsnPart2, string patientSsnPart3, string patientFirstName,
                                   string patientLastName, string heightFeet, string heightInches, string weight, string age)
        {

            Patient patient = new Patient();
            patient.patientSsnPart1 = patientSsnPart1;
            patient.patientSsnPart2 = patientSsnPart2;
            patient.patientSsnPart3 = patientSsnPart3;
            patient.patientFirstName = patientFirstName;
            patient.patientLastName = patientLastName;
            patient.heightFeet = heightFeet;
            patient.heightInches = heightInches;
            patient.weight = weight;
            patient.age = age;

            saveCaloriesXML(patient);
        }

        private static void Save(Patient patient)
        {
            saveCaloriesXML(patient);
        }

        private static void saveCaloriesXML(Patient patient)
        {
            bool isPatientDataValid = true;

            isPatientDataValid = validatePatientData(patient);




            if (isPatientDataValid == false)
            {
                throw new Exception("Invalid Output");
            }

            bool fileCreated = true;

            #region XML File Generation and Data Writing

            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(GetAssemblyDirectory() + @"\PatientsHistory.xml");
            }
            catch (FileNotFoundException ee)
            {
                //If file not found, set fileCreated to false and continue
                fileCreated = false;
            }
            if (!fileCreated)
            {
                document.LoadXml(
                "<PatientsHistory>" +
                "<patient ssn=\"" + patient.patientSsnPart1 + "-" + patient.patientSsnPart2 + "-" + patient.patientSsnPart3 + "\"" + " firstName=\"" + patient.patientFirstName + "\"" +
                " lastName=\"" + patient.patientLastName + "\"" + ">" +
                "<measurement date=\"" + DateTime.Now + "\"" + ">" +
                "<height>" + CommonFunction.GetHeightInInches(Convert.ToInt32(patient.heightFeet), Convert.ToInt32(patient.heightInches)).ToString() + "</height>" +
                "<weight>" + patient.weight + "</weight>" +
                "<age>" + patient.age + "</age>" +
                "<dailyCaloriesRecommended>" +
               CALORIES +
                "</dailyCaloriesRecommended>" +
                "<idealBodyWeight>" +
               IDEAL_WEIGHT +
                "</idealBodyWeight>" +
                "<distanceFromIdealWeight>" +
                DISTANCE_FROM_IDEAL_WEIGHT +
                "</distanceFromIdealWeight>" +
                "</measurement>" +
                "</patient>" +
                "</PatientsHistory>");
            }
            else
            {
                //Search for existing node for this patient
                XmlNode patientNode = null;
                foreach (XmlNode node in document.FirstChild.ChildNodes)
                {
                    foreach (XmlAttribute attrib in node.Attributes)
                    {
                        //We will use SSN to uniquely identify patient
                        if ((attrib.Name == "ssn") & (attrib.Value == patient.patientSsnPart1 + "-" + patient.patientSsnPart2 + "-" + patient.patientSsnPart3))
                        {
                            patientNode = node;
                        }
                    }
                }
                if (patientNode == null)
                {
                    //just clone any patient node and use it for the new patient node
                    XmlNode thisPatient =
                    document.DocumentElement.FirstChild.CloneNode(false);
                    thisPatient.Attributes["ssn"].Value = patient.patientSsnPart1 + "-" + patient.patientSsnPart2 + "-" + patient.patientSsnPart3;
                    thisPatient.Attributes["firstName"].Value = patient.patientFirstName;
                    thisPatient.Attributes["lastName"].Value = patient.patientLastName;
                    XmlNode measurement = document.DocumentElement.FirstChild["measurement"].CloneNode(true);
                    getMeasurementNode(patient, ref measurement);
                    thisPatient.AppendChild(measurement);
                    document.FirstChild.AppendChild(thisPatient);
                }
                else
                {
                    //If patient node found just clone any measurement
                    //and use it for the new measurement
                    XmlNode measurement = patientNode.FirstChild.CloneNode(true);
                    getMeasurementNode(patient, ref measurement);
                    patientNode.AppendChild(measurement);
                }
            }
            //Finally, save the xml to file
            document.Save(GetAssemblyDirectory() + @"\PatientsHistory.xml");
            #endregion XML File Generation and Data Writing
        }

        private static void getMeasurementNode(Patient patient , ref XmlNode measurement)
        {
            measurement.Attributes["date"].Value = DateTime.Now.ToString();
            measurement["height"].FirstChild.Value = CommonFunction.GetHeightInInches(Convert.ToInt32(patient.heightFeet), Convert.ToInt32(patient.heightInches)).ToString();
            measurement["weight"].FirstChild.Value = patient.weight;
            measurement["age"].FirstChild.Value = patient.age;
            measurement["dailyCaloriesRecommended"].FirstChild.Value = CALORIES;
            measurement["idealBodyWeight"].FirstChild.Value = IDEAL_WEIGHT;
            measurement["distanceFromIdealWeight"].FirstChild.Value = DISTANCE_FROM_IDEAL_WEIGHT;

        }

        private static bool validatePatientData(Patient patient )
        {
            bool isPatientDataValid = true;
            #region Patient Personal Input Data Validation
            int result;
            if ((!int.TryParse(patient.patientSsnPart1, out result)) |
                (!int.TryParse(patient.patientSsnPart2, out result)) |
                (!int.TryParse(patient.patientSsnPart3, out result)))
            {
                Console.WriteLine("You must enter valid SSN.");
                isPatientDataValid = false;
            }
            if (patient.patientFirstName.Trim().Length < 1)
            {
                Console.WriteLine("You must enter patient’s first name.");
                isPatientDataValid = false;
            }
            if (patient.patientLastName.Trim().Length < 1)
            {
                Console.WriteLine("You must enter patient’s last name.");
                isPatientDataValid = false;
            }
        #endregion Patient Personal Input Data Validation


            #region Patient General Data Validation
            double result1;
            if (!double.TryParse(patient.heightFeet, out result1))
            {
                Console.WriteLine("Feet must be a numeric value.");
                isPatientDataValid = false;
            }
            //Validate height (inches) is numeric value
            if (!double.TryParse(patient.heightInches, out result1))
            {
                Console.WriteLine("Inches must be a numeric value.");
                isPatientDataValid = false;
            }
            //Validate weight is numeric value
            if (!double.TryParse(patient.weight, out result1))
            {
                Console.WriteLine("Weight must be a numeric value.");
                isPatientDataValid = false;
            }
            //Validate age is numeric value
            if (!double.TryParse(patient.age, out result1))
            {
                Console.WriteLine("Age must be a numeric value.");
                isPatientDataValid = false;
            }
            if (!(Convert.ToDouble(patient.heightFeet) >= 5))
            {
                Console.WriteLine("Height has to be equal to or greater than 5 feet!");
                isPatientDataValid = false;
            }
            #endregion Patient General Data Validation
            

            return isPatientDataValid;
        }

       

        public static string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }


        public static string GetHistory()
        {
            return File.ReadAllText(GetAssemblyDirectory() + @"\PatientsHistory.xml");
        }



    }
}

