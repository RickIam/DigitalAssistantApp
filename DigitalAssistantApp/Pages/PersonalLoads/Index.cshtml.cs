using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DigitalAssistantApp;
using DigitalAssistantApp.DataBaseModels;
using OfficeOpenXml;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DigitalAssistantApp.Pages.PersonalLoads
{
    public class IndexModel : PageModel
    {
        private readonly DigitalAssistantApp.DadContext _context;

        public IndexModel(DigitalAssistantApp.DadContext context)
        {
            _context = context;
        }
        public IList<PersonalLoad> PersonalLoad { get; set; } = default!;

        private IList<PersonalLoad> GetPersonalLoad()
        {
            if (_context.PersonalLoads != null)
            {
                PersonalLoad = _context.PersonalLoads
                .Include(t => t.Loads)
                    .ThenInclude(f => f.Teacher)
                .Include(p => p.EducPlan)
                    .ThenInclude(b => b.Subject)
                .Include(f => f.EducPlan)
                    .ThenInclude(a => a.Spec)
                    .ThenInclude(d => d.Faculty)
                .Include(c => c.EducPlan)
                    .ThenInclude(d => d.Stream)
                .OrderBy(p => p.EducPlan.Semester)
                .ToList();
            }
            return PersonalLoad;
        }

        public void OnGet()
        {
            GetPersonalLoad();
        }

        public IActionResult OnGetExportToExcel()
        {
            GetPersonalLoad();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage("./wwwroot/Obraz.xlsx"))
            {
                var worksheet = package.Workbook.Worksheets.First();
                worksheet.Cells.Style.Font.Name = "Times New Roman";
                worksheet.Cells.Style.Font.Size = 11;
                worksheet.Column(2).Style.WrapText = true;
                worksheet.Row(7).Style.WrapText = true;
                for (int i = 2;i<=17;i++)
                {
                    worksheet.Cells[8, i, PersonalLoad.Count + 6, i].Style.Numberformat.Format = worksheet.Cells[7, i].Style.Numberformat.Format;
                    worksheet.Cells[8, i, PersonalLoad.Count + 6, i].Style.Border.Top.Style = worksheet.Cells[7, i].Style.Border.Top.Style;
                    worksheet.Cells[8, i, PersonalLoad.Count + 6, i].Style.Border.Bottom.Style = worksheet.Cells[7, i].Style.Border.Bottom.Style;
                    worksheet.Cells[8, i, PersonalLoad.Count + 6, i].Style.Border.Left.Style = worksheet.Cells[7, i].Style.Border.Left.Style;
                    worksheet.Cells[8, i, PersonalLoad.Count + 6, i].Style.Border.Right.Style = worksheet.Cells[7, i].Style.Border.Right.Style;
                    worksheet.Cells[8, i, PersonalLoad.Count + 6, i].Style.Font = worksheet.Cells[7, i].Style.Font;
                    worksheet.Cells[8, i, PersonalLoad.Count + 6, i].Style.WrapText = worksheet.Cells[7, i].Style.WrapText;
                    worksheet.Cells[8, i, PersonalLoad.Count + 6, i].Style.HorizontalAlignment = worksheet.Cells[7, i].Style.HorizontalAlignment;
                    worksheet.Cells[8, i, PersonalLoad.Count + 6, i].Style.VerticalAlignment = worksheet.Cells[7, i].Style.VerticalAlignment;
                }
                
                for (int i = 0; i < PersonalLoad.Count; i++)
                {
                    worksheet.Row(i + 7).CustomHeight = false;
                    worksheet.Row(i+7).Style.WrapText = true;
                    worksheet.Cells[i + 7, 2].Value = PersonalLoad[i]?.EducPlan?.Subject.SubjectName;
                    worksheet.Cells[i + 7, 3].Value = PersonalLoad[i]?.EducPlan?.SpecId;
                    worksheet.Cells[i + 7, 4].Value = PersonalLoad[i]?.Groups;
                    worksheet.Cells[i + 7, 5].Value = PersonalLoad[i]?.EducPlan?.Spec?.Faculty.FacultyName;
                    worksheet.Cells[i + 7, 6].Value = PersonalLoad[i]?.EducPlan?.Semester;
                    worksheet.Cells[i + 7, 7].Value = PersonalLoad[i]?.EducPlan?.Stream.StudCount;

                    string pattern = @"(\d+)\sгр\.\sна\s(лаб\.|пр\.)";

                    // Применяем регулярное выражение к тексту
                    MatchCollection matches = Regex.Matches(PersonalLoad[i]?.EducPlan?.Dept, pattern);

                    // Обрабатываем найденные совпадения
                    foreach (Match match in matches)
                    {
                        if (match.Success)
                        {
                            // Получаем значение числа из группы с индексом 1
                            string groupCount = match.Groups[1].Value;

                            // Преобразуем строку в число (int)
                            int numberOfGroups = int.Parse(groupCount);

                            // Получаем вид занятий из группы с индексом 2
                            string lessonType = match.Groups[2].Value;
                            if(lessonType=="лаб.")
                            {
                                worksheet.Cells[i + 7, 10].Value = numberOfGroups;
                            }
                            else
                            {
                                if(PersonalLoad[i]?.EducPlan?.PractiseCount==0)
                                {
                                    worksheet.Cells[i+7,10].Value = 0;
                                }
                            }
                            if(lessonType=="пр.")
                            {
                                worksheet.Cells[i + 7, 9].Value = numberOfGroups;
                            }
                            else
                            {
                                if (PersonalLoad[i]?.EducPlan?.PractiseCount == 0)
                                {
                                    worksheet.Cells[i + 7, 9].Value = 0;
                                }
                            }
                            Console.WriteLine($"{lessonType} : {numberOfGroups}");
                        }
                    }
                    if (PersonalLoad[i]?.EducPlan?.PractiseCount == 0)
                    {
                        worksheet.Cells[i + 7, 10].Value = 0;
                    }
                    if (PersonalLoad[i]?.EducPlan?.PractiseCount == 0)
                    {
                        worksheet.Cells[i + 7, 9].Value = 0;
                    }


                    worksheet.Cells[i + 7, 11].Value = PersonalLoad[i]?.EducPlan?.Att;
                    worksheet.Cells[i + 7, 12].Value = PersonalLoad[i]?.EducPlan?.LectionsCount;
                    worksheet.Cells[i + 7, 13].Value = PersonalLoad[i]?.EducPlan?.PractiseCount;
                    worksheet.Cells[i + 7, 14].Value = PersonalLoad[i]?.EducPlan?.LabWorkCount;
                    string TeacherInfo = "";
                    foreach (var Load in PersonalLoad[i].Loads)
                    {
                        TeacherInfo += Load?.Teacher?.FullName + " " + Load?.HoursCount + "\n";
                    }
                    worksheet.Cells[i + 7, 15].Value = TeacherInfo;
                }

                // Сохраните пакет в поток
                var stream = new MemoryStream(package.GetAsByteArray());

                // Отправьте файл Excel пользователю
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
            }
        }
    }
}
