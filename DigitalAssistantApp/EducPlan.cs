using System;
using System.Collections.Generic;

namespace DigitalAssistantApp;

public partial class EducPlan
{
    public int EducPlanId { get; set; }

    public int SubjectId { get; set; }

    public string Season { get; set; } = null!;

    public decimal? LectionsCount { get; set; }

    public decimal? PractiseCount { get; set; }

    public decimal? LabWorkCount { get; set; }

    public decimal? AudSrs { get; set; }

    public int? Zet { get; set; }

    public string? Att { get; set; }

    public decimal? Ha { get; set; }

    public decimal? Hkr { get; set; }

    public decimal? Hpr { get; set; }

    public decimal? Hat { get; set; }

    public decimal? H { get; set; }

    public int? VarRasch { get; set; }

    public int Semester { get; set; }

    public string? SpecId { get; set; }

    public string? Dept { get; set; }

    public int StreamId { get; set; }

    public virtual PersonalLoad? PersonalLoad { get; set; }

    public virtual Speciality? Spec { get; set; }

    public virtual Stream Stream { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
