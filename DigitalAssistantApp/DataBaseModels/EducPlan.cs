using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DigitalAssistantApp.DataBaseModels;

public partial class EducPlan
{
    [Key]
    public int EducPlanId { get; set; }

    public int SubjectId { get; set; }
    [Display(Name = "Сезон")]
    public string Season { get; set; } = null!;
    [Display(Name = "Лек")]
    public float? LectionsCount { get; set; }
    [Display(Name = "ПР")]
    public float? PractiseCount { get; set; }
    [Display(Name = "ЛР")]
    public float? LabWorkCount { get; set; }

    public float? AudSrs { get; set; }
    [Display(Name = "ZET")]
    public int? Zet { get; set; }
    [Display(Name = "Атт")]
    public string? Att { get; set; }

    public float? Ha { get; set; }

    public float? Hkr { get; set; }

    public float? Hpr { get; set; }

    public float? Hat { get; set; }

    public float? H { get; set; }

    public string? VarRasch { get; set; }
    [Display(Name = "Семестр")]
    public int Semester { get; set; }
    [Display(Name = "Шифр специальности")]
    public string? SpecId { get; set; }
    [Display(Name = "Кафедра")]
    public string? Dept { get; set; }

    public int StreamId { get; set; }


    public virtual Speciality? Spec { get; set; }

    public virtual Stream Stream { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
    //Возможны правки
    public virtual ICollection<PersonalLoad> PersonalLoads { get; set; } = new List<PersonalLoad>();
}
