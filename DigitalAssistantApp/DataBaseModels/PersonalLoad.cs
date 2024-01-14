using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DigitalAssistantApp.DataBaseModels;

public partial class PersonalLoad
{
    public int PersonalLoadId { get; set; }
    [Display(Name = "№ Группы")]
    [RegularExpression(@"^[0-9\s]+$", ErrorMessage = "Только цифры и пробелы")]
    public string? Groups { get; set; }
    [Display(Name = "Информация о преподавателе")]
    public string? TeachersInfo { get; set; }

    //Возможны правки

    public int EducPlanId { get; set; }

    //public int? TeacherId { get; set; }

    //public Teacher Teacher { get; set; } = null!;

    public virtual EducPlan EducPlan { get; set; } = null!;

    public virtual ICollection<Load> Loads { get; set; } = new List<Load>();
}
