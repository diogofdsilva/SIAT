using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Web.Mvc;

namespace SIAT.WebApplication.Models
{
    public class Occurrence
    {
        
        public int Id { get; set; }

        [StringLength(250, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 0)]
        [Display(Name = "Descrição")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Longitude")]
        public decimal Longitude { get; set; }

        [Required]
        [Display(Name = "Latitude")]
        public decimal Latitude { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Intensidade")]
        public string Intensity { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Nome da estrada")]
        public string WayName { get; set; }

        //public Brush IntensityColor
        //{
        //    get
        //    {
        //        return new SolidBrush(Color.Red);
        //    }
        //}
    }
}