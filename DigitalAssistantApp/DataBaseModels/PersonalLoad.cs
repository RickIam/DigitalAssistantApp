using System;
using System.Collections.Generic;

namespace DigitalAssistantApp.DataBaseModels;

public partial class PersonalLoad
{
    public int PersonalLoadId { get; set; }

    public string? Groups { get; set; } = null!;

    public string? TeachersInfo { get; set; }

    //Возможны правки

    public int EducPlanId { get; set; }

    public int? TeacherId { get; set; }

    public Teacher Teacher { get; set; } = null!;
    
    public virtual EducPlan EducPlan { get; set; } = null!;
}
