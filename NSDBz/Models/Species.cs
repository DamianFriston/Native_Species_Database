using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NSDBz.Models
{
    public class Species
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please provide an image URL.")]
        public string ImgUrl { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a Class.")]
        public string Class { get; set; }

        [Required]
        public string Bio { get; set; }

        [Required]
        public int Legs { get; set; }

        [Required(ErrorMessage = "Please select a category of Toxicity.")]
        public string Toxicity { get; set; }


        
        public virtual ICollection<Country> Countries { get; set; }
    }
}