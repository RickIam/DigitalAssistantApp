using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace DigitalAssistantApp.DataBaseModels;

public partial class Load
{
    public int LoadId { get; set; }
    [Required(ErrorMessage = "Выберите преподавателя")]
    public int? TeacherId { get; set; }

    public int? PersonalLoadId { get; set; }
    [Display(Name = "Часы")]
    [Required(ErrorMessage = "Введите кол-во часов")]
    [RegularExpression(@"^\d+(\,\d+)?$", ErrorMessage = "Только число целое или десятичное через ','")]
    public float? HoursCount { get; set; } = null!;

    public Teacher? Teacher { get; set; }

    public PersonalLoad? PersonalLoad { get; set; }
}