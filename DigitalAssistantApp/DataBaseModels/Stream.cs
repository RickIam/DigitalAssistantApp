using System;
using System.Collections.Generic;

namespace DigitalAssistantApp.DataBaseModels;

public partial class Stream
{
    public int? StudCount { get; set; }

    public int? StudVb { get; set; }

    public int? StudForeign { get; set; }

    public int Semester { get; set; }

    public int? Group { get; set; }

    public int StreamId { get; set; }
    
    public string? FOb { get; set; }

    public virtual ICollection<EducPlan> EducPlans { get; set; } = new List<EducPlan>();
}
