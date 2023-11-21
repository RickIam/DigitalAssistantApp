using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssistantApp.DataBaseModels;

public partial class Teacher
{
    public int TeacherId { get; set; }
    [Display(Name = "Имя")]
    public string Firstname { get; set; } = null!;
    [Display(Name = "Фамилия")]
    public string Lastname { get; set; } = null!;
    [Display(Name = "Отчество")]
    public string? PatronymicName { get; set; }
    [Display(Name = "Кафедра")]
    public string? Department { get; set; }

    public virtual PersonalLoad? PersonalLoad { get; set; }
}
