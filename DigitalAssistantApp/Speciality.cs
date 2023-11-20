using System;
using System.Collections.Generic;

namespace DigitalAssistantApp;

public partial class Speciality
{
    public string SpecId { get; set; } = null!;

    public int FacultyId { get; set; }

    public virtual ICollection<EducPlan> EducPlans { get; set; } = new List<EducPlan>();

    public virtual Faculty Faculty { get; set; } = null!;
}
