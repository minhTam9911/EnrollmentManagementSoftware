﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentManagementSoftware.Models;

public partial class TuitionType
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]	
	public int Id { get; set; }
	public string? Name { get; set; }
	public decimal? Percent { get; set; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public virtual User? CreateBy { get; set; }
}
