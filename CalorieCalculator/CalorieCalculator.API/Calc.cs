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
            CALORIES= "";
            /* Validate User Input: */
            //Validate height (feet) is numeric value

            #region Input Validation
            double result;
            if (!double.TryParse(heightFeet, out result))
            {
                throw new Exception("Feet must be a numeric value.");
            }
            //Validate height (inches) is numeric value
            if (!double.TryParse(heightInches, out result))
            {
                throw new Exception("Inches must be a numeric value.");
            }
            //Validate weight is numeric value
            if (!double.TryParse(weight, out result))
            {
                throw new Exception("Weight must be a numeric value.");
            }
            //Validate age is numeric value
            if (!double.TryParse(age, out result))
            {
                throw new Exception("Age must be a numeric value.");
            }
            if (!(Convert.ToDouble(heightFeet) >= 5))
            {
                throw new Exception("Height has to be equal to or greater than 5 feet!");
            }
            #endregion Input Validation


            #region Calories Calculation
            /*End validation*/
            if (sex == The_sex.Male)
            {
                CALORIES = (66
                + (6.3 * Convert.ToDouble(weight))
                + (12.9 * ((Convert.ToDouble(heightFeet) * 12) + Convert.ToDouble(heightInches)))
                - (6.8 * Convert.ToDouble(age))).ToString();

                #region  Calculate ideal body weight
                //Calculate ideal body weight

                IDEAL_WEIGHT = ((50 +
                (2.3 * (((Convert.ToDouble(heightFeet) - 5) * 12)
                + Convert.ToDouble(heightInches)))) * 2.2046).ToString();
                #endregion
            }
            else
            {
                CALORIES = (655
                + (4.3 * Convert.ToDouble(weight))
                + (4.7 * ((Convert.ToDouble(heightFeet) * 12)
                + Convert.ToDouble(heightInches)))
                - (4.7 * Convert.ToDouble(age))).ToString();
                #region  Calculate ideal body weight
                //Calculate ideal body weight
                IDEAL_WEIGHT = ((45.5 +
                (2.3 * (((Convert.ToDouble(heightFeet) - 5) * 12)
                + Convert.ToDouble(heightInches)))) * 2.2046).ToString();
                #endregion
            }
            #endregion Calories Calculation


            #region Calculate and display distance from ideal weight
            //Calculate and display distance from ideal weight
            DISTANCE_FROM_IDEAL_WEIGHT = (Convert.ToDouble(weight) - Convert.ToDouble(IDEAL_WEIGHT)).ToString();
            #endregion

        }

        public enum The_sex
        {
            Male,
            Female
        }

        public static void Save(string patientSsnPart1,string patientSsnPart2, string patientSsnPart3, string patientFirstName,  
                                   string patientLastName,  string heightFeet, string heightInches, string weight, string age)
        {


            bool PatientPhysicalDataValidation = true;
            bool PatientPersonalDataValidation = true;


            #region Patient Personal Input Data Validation
            int result;
            if ((!int.TryParse(patientSsnPart1, out result)) |
                (!int.TryParse(patientSsnPart2, out result)) |
                (!int.TryParse(patientSsnPart3, out result)))
            {
                Console.WriteLine("You must enter valid SSN.");
                PatientPersonalDataValidation = false;
            }
            if (patientFirstName.Trim().Length < 1)
            {
                Console.WriteLine("You must enter patient’s first name.");
                PatientPersonalDataValidation = false;
            }
            if (patientLastName.Trim().Length < 1)
            {
                Console.WriteLine("You must enter patient’s last name.");
                PatientPersonalDataValidation = false;
            }
            #endregion Patient Personal Input Data Validation


            #region Patient General Data Validation
            double result1;
            if (!double.TryParse(heightFeet, out result1))
            {
                Console.WriteLine("Feet must be a numeric value.");
                PatientPhysicalDataValidation = false;
            }
            //Validate height (inches) is numeric value
            if (!double.TryParse(heightInches, out result1))
            {
                Console.WriteLine("Inches must be a numeric value.");
                PatientPhysicalDataValidation = false;
            }
            //Validate weight is numeric value
            if (!double.TryParse(weight, out result1))
            {
                Console.WriteLine("Weight must be a numeric value.");
                PatientPhysicalDataValidation = false;
            }
            //Validate age is numeric value
            if (!double.TryParse(age, out result1))
            {
                Console.WriteLine("Age must be a numeric value.");
                PatientPhysicalDataValidation = false;
            }
            if (!(Convert.ToDouble(heightFeet) >= 5))
            {
                Console.WriteLine("Height has to be equal to or greater than 5 feet!");
                PatientPhysicalDataValidation = false;
            }
            /*End validation*/


            #endregion Patient General Data Validation


            if (PatientPersonalDataValidation == false || PatientPhysicalDataValidation == false)
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
                "<patient ssn=\"" + patientSsnPart1 + "-" + patientSsnPart2 + "-" + patientSsnPart3+ "\"" + " firstName=\"" + patientFirstName + "\"" +
                " lastName=\"" + patientLastName + "\"" + ">" +
                "<measurement date=\"" + DateTime.Now + "\"" + ">" +
                "<height>" + ((Convert.ToInt32(heightFeet) * 12) + heightInches).ToString() + "</height>" +
                "<weight>" + weight + "</weight>" +
                "<age>" + age + "</age>" +
                "<dailyCaloriesRecommended>" +
               CALORIES+
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
                        if ((attrib.Name == "ssn") & (attrib.Value == patientSsnPart1 + "-" + patientSsnPart2 + "-" + patientSsnPart3))
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
                    thisPatient.Attributes["ssn"].Value = patientSsnPart1 + "-" + patientSsnPart2 + "-" + patientSsnPart3;
                    thisPatient.Attributes["firstName"].Value = patientFirstName;
                    thisPatient.Attributes["lastName"].Value = patientLastName;
                    XmlNode measurement = document.DocumentElement.FirstChild["measurement"].CloneNode(true);
                    measurement.Attributes["date"].Value = DateTime.Now.ToString();
                    measurement["height"].FirstChild.Value = ((Convert.ToInt32(heightFeet) * 12) + Convert.ToInt32(heightInches)).ToString();
                    measurement["weight"].FirstChild.Value = weight;
                    measurement["age"].FirstChild.Value = age;
                    measurement["dailyCaloriesRecommended"].FirstChild.Value = CALORIES;
                    measurement["idealBodyWeight"].FirstChild.Value = IDEAL_WEIGHT;
                    measurement["distanceFromIdealWeight"].FirstChild.Value = DISTANCE_FROM_IDEAL_WEIGHT;
                    thisPatient.AppendChild(measurement);
                    document.FirstChild.AppendChild(thisPatient);
                }
                else
                {
                    //If patient node found just clone any measurement
                    //and use it for the new measurement
                    XmlNode measurement = patientNode.FirstChild.CloneNode(true);
                    measurement.Attributes["date"].Value = DateTime.Now.ToString();
                    measurement["height"].FirstChild.Value = ((Convert.ToInt32(heightFeet) * 12) + Convert.ToInt32(heightInches)).ToString();
                    measurement["weight"].FirstChild.Value = weight;
                    measurement["age"].FirstChild.Value = age;
                    measurement["dailyCaloriesRecommended"].FirstChild.Value = CALORIES;
                    measurement["idealBodyWeight"].FirstChild.Value = IDEAL_WEIGHT;
                    measurement["distanceFromIdealWeight"].FirstChild.Value = DISTANCE_FROM_IDEAL_WEIGHT;
                    patientNode.AppendChild(measurement);
                }
            }
            //Finally, save the xml to file
            document.Save(GetAssemblyDirectory() +  @"\PatientsHistory.xml");
            #endregion XML File Generation and Data Writing
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

