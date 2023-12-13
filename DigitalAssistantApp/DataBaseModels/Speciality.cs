using System;
using System.Collections.Generic;

namespace DigitalAssistantApp.DataBaseModels;

public partial class Speciality
{
    public string SpecId { get; set; } = null!;

    public int FacultyId { get; set; }

    public virtual ICollection<EducPlan> EducPlans { get; set; } = new List<EducPlan>();
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
    public virtual Faculty Faculty { get; set; } = null!;
}
