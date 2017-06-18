using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCalculator.API
{
    public class Patient
    {
        private string _patientSsnPart1;
        private string _patientSsnPart2;
        private string _patientSsnPart3;
        private string _patientFirstName;
        private string _patientLastName;
        private double _heightFeet;
        private double _heightInches;
        private double _weight;
        private double _age;
        private Calc.The_sex _sex;
        private double number;
        private int intNumber;

        public Calc.The_sex sex
        {
            get
            {
                return _sex;
            }

            set
            { // validating value
                _sex = value;
            }
        }

        public string patientSsnPart1
        {
            get
            {
                return _patientSsnPart1;
            }

            set
            { // validating value
                _patientSsnPart1 = value;
            }
        }

        public string patientSsnPart2
        {
            get
            {
                return _patientSsnPart2;
            }

            set
            { // validating value
                _patientSsnPart2 = value;
            }
        }

        public string patientSsnPart3
        {
            get
            {
                return _patientSsnPart3;
            }

            set
            { // validating value
                _patientSsnPart3 = value;
            }
        }

        public string patientFirstName
        {
            get
            {
                return _patientFirstName;
            }

            set
            { // validating value
                _patientFirstName = value;
            }
        }

        public string patientLastName
        {
            get
            {
                return _patientLastName;
            }

            set
            { // validating value
                _patientLastName = value;
            }
        }

        public string heightFeet
        { // public property
            get
            {
                return _heightFeet.ToString();
            }

            set
            { // validating value
                if (Double.TryParse(value, out number))
                {
                    _heightFeet = number;
                }
                else
                { // throw exception if invalid value
                    throw new Exception("Feet must be a numeric value.");
                }
            }
        }

        public string heightInches
        { // public property
            get
            {
                return _heightInches.ToString();
            }

            set
            { // validating value
                if (Double.TryParse(value, out number))
                {
                    _heightInches = number;
                }
                else
                { // throw exception if invalid value
                    throw new Exception("Inches must be a numeric value.");
                }
            }
        }

        public string weight
        { // public property
            get
            {
                return _weight.ToString();
            }

            set
            { // validating value
                if (Double.TryParse(value, out number))
                {
                    _weight = number;
                }
                else
                { // throw exception if invalid value
                    throw new Exception("Weight must be a numeric value.");
                }
            }
        }

        public string age
        { // public property
            get
            {
                return _age.ToString();
            }

            set
            { // validating value
                if (Double.TryParse(value, out number))
                {
                    if (number >= 5)
                    {
                        _age = number;
                    }
                    else { throw new Exception("Height has to be equal to or greater than 5 feet!"); }

                }
                else
                { // throw exception if invalid value
                    throw new Exception("Age must be a numeric value.");
                }
            }
        }

    }
}
