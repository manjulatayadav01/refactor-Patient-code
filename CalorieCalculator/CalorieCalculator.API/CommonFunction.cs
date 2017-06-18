using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCalculator.API
{
    public static class CommonFunction
    {
        public static double GetCaloryConsumption(double heightFeet, double heightInches, double weight, double age, Calc.The_sex sex)
        {
            double Calories = 0;
            double heightInInches = Convert.ToDouble(GetHeightInInches(Convert.ToInt32(heightFeet), Convert.ToInt32(heightInches)));


            if (sex == Calc.The_sex.Male)
            {
                Calories = 66
                + (6.3 * weight)
                + (12.9 * (heightInInches)
                - (6.8 * age));
            }
            else
            {
                Calories = 655
                + (4.3 * weight)
                + (4.7 * (heightInInches)
                - (4.7 * age));
            }

            return Calories;

        }


        public static double GetIdealWeight(double heightFeet, double heightInches, Calc.The_sex sex)
        {
            double idealWeight = 0;
            double idealWeightConstant = 0;
            if (sex == Calc.The_sex.Male)
            {
                idealWeightConstant = 50;
            }
            else
            {
                idealWeightConstant = 45.5;
            }

            idealWeight = ((idealWeightConstant +
                (2.3 * (((heightFeet - 5) * 12)
                + heightInches))) * 2.2046);

            return idealWeight;

        }

        public static int GetHeightInInches(int heightFeet, int heightInches)
        {
            return ((heightFeet * 12) + heightInches);
        }

    }
}
