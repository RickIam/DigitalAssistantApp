using System;
using System.Collections.Generic;



namespace DigitalAssistantApp.DataBaseModels;

public partial class Load
{
    public int LoadId { get; set; }

    public int? TeacherId { get; set; }

    public int? PersonalLoadId { get; set; }

    public float? HoursCount { get; set; }

    public Teacher? Teacher { get; set; }

    public PersonalLoad? PersonalLoad { get; set; }
}