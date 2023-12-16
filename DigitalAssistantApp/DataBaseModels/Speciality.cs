using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DigitalAssistantApp.DataBaseModels;

public partial class Speciality
{
    [Display(Name = "Шифр специальности")]
    public string SpecId { get; set; } = null!;

    public int FacultyId { get; set; }

    public virtual ICollection<EducPlan> EducPlans { get; set; } = new List<EducPlan>();

    public virtual Faculty Faculty { get; set; } = null!;
}
