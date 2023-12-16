using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DigitalAssistantApp.DataBaseModels;

public partial class Stream
{
    [Display(Name = "Кол.студ")]
    public int? StudCount { get; set; }

    public int? StudVb { get; set; }

    public int? StudForeign { get; set; }
    [Display(Name = "Семестр")]
    public int Semester { get; set; }
    [Display(Name = "Кол-во групп")]
    public int? Group { get; set; }

    public int StreamId { get; set; }
    [Display(Name = "Форма обучения")]
    public string? FOb { get; set; }

    public virtual ICollection<EducPlan> EducPlans { get; set; } = new List<EducPlan>();
}
