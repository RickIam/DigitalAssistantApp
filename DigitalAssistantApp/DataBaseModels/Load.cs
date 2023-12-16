using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace DigitalAssistantApp.DataBaseModels;

public partial class Load
{
    public int LoadId { get; set; }

    public int? TeacherId { get; set; }

    public int? PersonalLoadId { get; set; }
    [Display(Name = "Часы")]
    public float? HoursCount { get; set; }

    public Teacher? Teacher { get; set; }

    public PersonalLoad? PersonalLoad { get; set; }
}