using System;
using System.Collections.Generic;

namespace DigitalAssistantApp.DataBaseModels;

public partial class EducPlan
{
    public int EducPlanId { get; set; }

    public int SubjectId { get; set; }

    public string Season { get; set; } = null!;

    public float? LectionsCount { get; set; }

    public float? PractiseCount { get; set; }

    public float? LabWorkCount { get; set; }

    public float? AudSrs { get; set; }

    public int? Zet { get; set; }

    public string? Att { get; set; }

    public float? Ha { get; set; }

    public float? Hkr { get; set; }

    public float? Hpr { get; set; }

    public float? Hat { get; set; }

    public float? H { get; set; }

    public string? VarRasch { get; set; }

    public int Semester { get; set; }

    public string? SpecId { get; set; }

    public string? Dept { get; set; }

    public int StreamId { get; set; }


    public virtual Speciality? Spec { get; set; }

    public virtual Stream Stream { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
    //Возможны правки
    public virtual ICollection<PersonalLoad> PersonalLoads { get; set; } = new List<PersonalLoad>();
}
