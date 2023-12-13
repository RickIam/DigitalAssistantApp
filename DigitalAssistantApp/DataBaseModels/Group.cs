using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAssistantApp.DataBaseModels;

public partial class Group
{
    [Display(Name = "Номер группы")]
    public string GroupNumber { get; set; } = null!;
    [Display(Name = "Код специальности")]
    public string SpecId { get; set; } = null!;
    public virtual Speciality? Speciality { get; set; }
    
}



