﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DigitalAssistantApp.DataBaseModels;

public partial class Subject
{
    public int SubjectId { get; set; }
    [Display(Name = "Дисциплина")]
    public string SubjectName { get; set; } = null!;

    public virtual ICollection<EducPlan> EducPlans { get; set; } = new List<EducPlan>();
}
