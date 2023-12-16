using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DigitalAssistantApp.DataBaseModels;

public partial class Faculty
{
    public int FacultyId { get; set; }
    [Display(Name = "Кафедра")]
    public string FacultyName { get; set; } = null!;

    public virtual ICollection<Speciality> Specialities { get; set; } = new List<Speciality>();
}
