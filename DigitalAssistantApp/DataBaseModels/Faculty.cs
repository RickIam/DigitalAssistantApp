using System;
using System.Collections.Generic;

namespace DigitalAssistantApp.DataBaseModels;

public partial class Faculty
{
    public int FacultyId { get; set; }

    public string FacultyName { get; set; } = null!;

    public virtual ICollection<Speciality> Specialities { get; set; } = new List<Speciality>();
}
