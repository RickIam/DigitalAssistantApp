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
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Нагрузка 1");
                worksheet.Cells.Style.Font.Name = "Times New Roman";
                worksheet.Cells.Style.Font.Size = 8;

                worksheet.Cells[1, 1].Value = "Дисциплина";
                worksheet.Cells[1, 2].Value = "Шифр специальноси";
                worksheet.Cells[1, 3].Value = "№ группы";
                worksheet.Cells[1, 4].Value = "Сем";
                worksheet.Cells[1, 5].Value = "Ко-во групп(по нагрузке)";
                worksheet.Cells[1, 6].Value = "Кол.студ";
                worksheet.Cells[1, 7].Value = "Атт";
                worksheet.Cells[1, 8].Value = "ZET";
                worksheet.Cells[1, 9].Value = "Лек";
                worksheet.Cells[1, 10].Value = "ПЗ";
                worksheet.Cells[1, 11].Value = "ЛР";
                worksheet.Cells[1, 12].Value = "ФИО преподавателя, звание, должность";
                worksheet.Cells[1, 13].Value = "ФИО";
                worksheet.Cells[1, 14].Value = "Часы";
                worksheet.Cells[1, 15].Value = "ФИО";
                worksheet.Cells[1, 16].Value = "Часы";
                worksheet.Cells[1, 17].Value = "ФИО";
                worksheet.Cells[1, 18].Value = "Часы";
                for (int i = 0; i < PersonalLoad.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = PersonalLoad[i]?.EducPlan?.Subject.SubjectName;
                    worksheet.Cells[i + 2, 2].Value = PersonalLoad[i]?.EducPlan?.SpecId;
                    worksheet.Cells[i + 2, 3].Value = PersonalLoad[i].Groups;
                    worksheet.Cells[i + 2, 4].Value = PersonalLoad[i]?.EducPlan?.Semester;
                    worksheet.Cells[i + 2, 5].Value = PersonalLoad[i]?.EducPlan?.Stream.Group;
                    worksheet.Cells[i + 2, 6].Value = PersonalLoad[i]?.EducPlan?.Stream.StudCount;
                    worksheet.Cells[i + 2, 7].Value = PersonalLoad[i]?.EducPlan?.Att;
                    worksheet.Cells[i + 2, 8].Value = PersonalLoad[i]?.EducPlan?.Zet;
                    worksheet.Cells[i + 2, 9].Value = PersonalLoad[i]?.EducPlan?.LectionsCount;
                    worksheet.Cells[i + 2, 10].Value = PersonalLoad[i]?.EducPlan?.PractiseCount;
                    worksheet.Cells[i + 2, 11].Value = PersonalLoad[i]?.EducPlan?.LabWorkCount;
                    worksheet.Cells[i + 2, 12].Value = PersonalLoad[i].TeachersInfo;
                    int j = 13;
                    foreach (var Load in PersonalLoad[i].Loads)
                    {
                        worksheet.Cells[i + 2, j].Value = Load?.Teacher?.FullName;
                        worksheet.Cells[i + 2, j + 1].Value = Load?.HoursCount;
                        j += 2;
                    }

                }

                // Сохраните пакет в поток
                var stream = new MemoryStream(package.GetAsByteArray());

                // Отправьте файл Excel пользователю
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
            }
        }
    }
}
