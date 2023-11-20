using System;
using System.Collections.Generic;

namespace DigitalAssistantApp;

public partial class PersonalLoad
{
    public int PersonalLoadId { get; set; }

    public virtual Teacher PersonalLoad1 { get; set; } = null!;

    public virtual EducPlan PersonalLoadNavigation { get; set; } = null!;
}
