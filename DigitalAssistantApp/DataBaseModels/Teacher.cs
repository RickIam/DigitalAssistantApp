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
    //Возможны правки
    public virtual ICollection<Load> Loads { get; set; } = new List<Load>();


    [NotMapped]
    [Display(Name = "ФИО")]
    public string FullName
    {
        get
        {
            if (string.IsNullOrWhiteSpace(PatronymicName))
            {
                return $"{Lastname} {Firstname}";
            }
            else
            {
                return $"{Lastname} {Firstname} {PatronymicName}";
            }
        }
    }
}
