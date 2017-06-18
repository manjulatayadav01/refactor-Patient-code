using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace FeetImport.Repository
{
    class Fleet
    {
        [Required]
        public string  VehicleName { get; set; }

        [Required]
        [RegularExpression(@"\(\d{4}\)", ErrorMessage = "Make should be alpha numeric.")]
         
        public int Year { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "Make should be alpha numeric.")]
        public string Make { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "Model should be alpha numeric.")]
        public string Model { get; set; }

        [RegularExpression("[A-HJ-NPR-Z0-9]{13}[0-9]{4}", ErrorMessage = "Invalid Vehicle Identification Number Format.")]
        public string VINNumber { get; set; }

        public string Country { get; set; }

        public string LicensePlateNumber { get; set; }

        [RegularExpression("^[A-Z]{2}[0-9]{4}[A-Z]{2}$", ErrorMessage = "Invalid Nautical Registration Number Format.")]
        public string NauticalRegistrationNumber { get; set; }



    }
}
