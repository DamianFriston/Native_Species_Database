using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NSDBz.Models
{
    public class Country
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please provide an image URL.")]
        public string ImgUrl { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Size { get; set; }

        [Required(ErrorMessage = "Please select one.")]
        public string Hemisphere { get; set; }

        
        public virtual ICollection<Species> Species { get; set; }

    }
}