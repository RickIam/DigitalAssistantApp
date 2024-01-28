using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssistantApp.DataBaseModels;

public partial class Teacher
{
    public int TeacherId { get; set; }
    [Display(Name = "Имя")]
    [Required(ErrorMessage = "Не указано имя")]
    [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\s]+$", ErrorMessage = "Только буквы и пробелы")]
    [StringLength(50, ErrorMessage = "Максимум 50 символов")]
    public string Firstname { get; set; } = null!;
    [Display(Name = "Фамилия")]
    [Required(ErrorMessage = "Не указана Фамилия")]
    [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\s]+$", ErrorMessage = "Только буквы и пробелы")]
    [StringLength(50, ErrorMessage = "Максимум 50 символов")]
    public string Lastname { get; set; } = null!;
    [Display(Name = "Отчество")]
    [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\s]+$", ErrorMessage = "Только буквы и пробелы")]
    [StringLength(50, ErrorMessage = "Максимум 50 символов")]
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
