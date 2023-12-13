using System;
using System.Collections.Generic;

namespace DigitalAssistantApp.DataBaseModels;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string SubjectName { get; set; } = null!;

    public virtual ICollection<EducPlan> EducPlans { get; set; } = new List<EducPlan>();
    
}
