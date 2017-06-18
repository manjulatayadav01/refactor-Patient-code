using System;
using CalorieCalculator;
using CalorieCalculator.API;

namespace CalorieCounterTestHarness
{
    class Program
    {

        public static void Main(string[] args)
        {
            string firstName = "Bob";
            string lastName = "Smith";
            string ssnPart1 = "123";
            string ssnPart2 = "33";
            string ssnPart3 = "1234";
            Calc.The_sex sex = Calc.The_sex.Male;
            string age = "33";
            string heightFeet = "5";
            string heightInches = "10";
            string weight = "250";


            Calc.Calculate(heightFeet, heightInches, weight, age, Calc.The_sex.Male);
            Console.WriteLine("Patient: {0} {1}, SSN: {2}-{3}-{4}", firstName, 
                                                                    lastName, 
                                                                    ssnPart1, 
                                                                    ssnPart2, 
                                                                    ssnPart3);
 
            
            Console.WriteLine("Stats: {0}'{1}\", {2} year old {3} weighing {4} lbs", heightFeet, 
                                                                                     heightInches,
                                                                                     age,
                                                                                     Enum.GetName(typeof(Calc.The_sex), sex), 
                                                                                     weight); 

            Console.WriteLine("Calcuation: Ideal Weight = {0}, Distance From Ideal Weight = {1}, Calories = {2}", Calc.IDEAL_WEIGHT, 
                                                                                                                  Calc.DISTANCE_FROM_IDEAL_WEIGHT, 
                                                                                                                  Calc.CALORIES);


            Calc.Save(ssnPart1, ssnPart2, ssnPart3, firstName, lastName, heightFeet, heightInches, weight, age);


            string history = Calc.GetHistory();
            Console.WriteLine();
            Console.WriteLine("Here is your previous history:");
            Console.WriteLine(history);

            Console.WriteLine("Press enter to quit");
            Console.ReadLine();
        }

    }
}
