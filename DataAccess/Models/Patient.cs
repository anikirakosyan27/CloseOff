using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Models
{
   public class Patient
    {
		public int Id { get; set; }
		[Required]
		[StringLength(50)]
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public virtual ICollection<Isolated> Isolateds { get; set; }

	}
}
