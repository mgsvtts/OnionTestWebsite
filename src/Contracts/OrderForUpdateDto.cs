using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class OrderForUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter an order number")]

        public string Number { get; set; }

        [Required(ErrorMessage = "Choose a provider")]
        public int ProviderId { get; set; }
    }
}
