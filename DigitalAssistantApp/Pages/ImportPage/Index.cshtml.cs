using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssistantApp.Pages.ImportPage
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IFormFile Upload { get; set; }
        
        private readonly DigitalAssistantApp.DadContext _context;

        public IndexModel(DigitalAssistantApp.DadContext context)
        {
            _context = context;
        }
        public async Task OnPostAsync()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", Upload.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }
            var fileInfo = new FileInfo(file);
            string importfilepath = "./wwwroot/Files/" + Upload.FileName;
            int LastRowNumber = ExcelPrepare(fileInfo);
            var importfileinfo = new FileInfo(importfilepath);
            PreparedFileImport(importfileinfo, LastRowNumber);
            ValuesToTables();
            fileInfo.Delete();
        }
        private void ValuesToTables()
        {
            using (var connection = new NpgsqlConnection("Server=172.20.7.9;Port=5432;Database=DAD;User Id=superuser;Password=rootUSER"))
            {
                connection.Open();
                //������������� ������ �� ��������
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        INSERT INTO public.faculty (faculty_name)
                        SELECT DISTINCT ""�����"" FROM public.nagruzka1 n
                        WHERE NOT EXISTS (
                            SELECT 1 FROM public.faculty f
                            WHERE f.faculty_name = n.""�����""
                        );
                        INSERT INTO public.speciality (spec_id, faculty_id)
                        SELECT DISTINCT n.""��_��"", f.faculty_id
                        FROM public.nagruzka1 n
                        JOIN public.faculty f ON n.""�����"" = f.faculty_name
                        ON CONFLICT (spec_id) DO NOTHING;
                        INSERT INTO public.stream (stud_count, stud_vb, stud_foreign, semester, ""group"", f_ob)
                        SELECT DISTINCT ""���_����"", ""���_��"", ""���_���"", ""���"", ""���"", ""�_��""
                        FROM public.nagruzka1;
                        INSERT INTO public.subject (subject_name)
                        SELECT DISTINCT n.""�������""
                        FROM public.nagruzka1 n
                        WHERE NOT EXISTS (
                            SELECT 1 FROM public.subject s
                            WHERE s.subject_name = n.""�������""
                        );
                        WITH educ_plan_in AS (INSERT INTO public.educ_plan (
                            subject_id, season, lections_count, practise_count, lab_work_count, aud_srs, zet, att,
                            ha, hkr, hpr, hat, h, var_rasch, semester, spec_id, dept, stream_id
                        )
                        SELECT
                            s.subject_id, n.""�����"", n.""���"", n.""��"", n.""��"", n.""���_���"", n.zet, n.""���"",
                            n.""HA"", n.""HKR"", n.""HPR"", n.""HAT"", n.""H"", n.""���_����"", n.""���"", sp.spec_id, n.""DEPT"", st.stream_id
                        FROM
                            public.nagruzka1 n
                        JOIN public.subject s ON n.""�������"" = s.subject_name
                        JOIN public.speciality sp ON n.""��_��"" = sp.spec_id
                        JOIN public.stream st ON n.""���"" = st.semester AND n.""���"" = st.""group"" AND n.""�_��"" = st.f_ob AND n.""���_����""=st.stud_count AND n.""���_��""=st.stud_vb AND n.""���_���""=st.stud_foreign
                        RETURNING educ_plan_id)

                        INSERT INTO public.personal_load (educ_plan_id)
                        SELECT
                            educ_plan_id
                        FROM educ_plan_in";
                    command.ExecuteNonQuery();
                }
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"truncate nagruzka1 restart identity;";
                    command.ExecuteNonQuery();
                }
                connection.Close(); //�������� ����������
            }
        }
        private int ExcelPrepare(FileInfo fileInfo)
        {
            int endrow = -1;
            using (ExcelPackage package = new ExcelPackage(fileInfo.FullName))
            {
                var worksheet = package.Workbook.Worksheets[0];
                worksheet.DeleteRow(1);
                for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                {
                    if (worksheet.Cells[row, 1].Value == null)
                    {
                        if (endrow == -1)
                        {
                            endrow = row;
                        }
                        for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                        {
                            worksheet.Cells[row, col].Value = null;
                        }
                    }
                }
                package.Save();
            }
            return endrow;
        }
        public void PreparedFileImport(FileInfo file,int LastRowNumber)
        {
            if (file != null && file.Length > 0)
            {
                using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        for (int row = 2; row < LastRowNumber; row++)
                        {
                            Nagruzka1 data = new Nagruzka1();
                            data.����� = worksheet.Cells[row, 1].Value?.ToString();
                            data.��� = worksheet.Cells[row, 2].Value?.ToString();
                            data.��� = worksheet.Cells[row, 3].Value?.ToString();
                            data.������� = worksheet.Cells[row, 4].Value?.ToString();
                            data.����� = worksheet.Cells[row, 5].Value?.ToString();
                            data.���� = worksheet.Cells[row, 6].Value?.ToString();
                            data.��� = Convert.ToInt32(worksheet.Cells[row, 7].Value); 
                            data.��� = Convert.ToInt32(worksheet.Cells[row, 8].Value);
                            data.������� = Convert.ToInt32(worksheet.Cells[row, 9].Value);
                            data.����� = Convert.ToInt32(worksheet.Cells[row, 10].Value);
                            data.������ = Convert.ToInt32(worksheet.Cells[row, 11].Value);
                            data.������� = worksheet.Cells[row, 12].Value?.ToString();
                            data.��� = worksheet.Cells[row, 13].Value?.ToString();
                            data.Zet = (float?)Convert.ToDouble(worksheet.Cells[row, 14].Value);
                            data.��� = (float?)Convert.ToDouble(worksheet.Cells[row, 15].Value);
                            data.�� = (float?)Convert.ToDouble(worksheet.Cells[row, 16].Value);
                            data.�� = (float?)Convert.ToDouble(worksheet.Cells[row, 17].Value);
                            data.������ = (float?)Convert.ToDouble(worksheet.Cells[row, 18].Value);
                            data.Ha = (float?)Convert.ToDouble(worksheet.Cells[row, 19].Value);
                            data.Hkr = (float?)Convert.ToDouble(worksheet.Cells[row, 20].Value);
                            data.Hpr = (float?)Convert.ToDouble(worksheet.Cells[row, 21].Value);
                            data.Hat = (float?)Convert.ToDouble(worksheet.Cells[row, 22].Value);
                            data.H = (float?)Convert.ToDouble(worksheet.Cells[row, 23].Value);
                            data.Dept = worksheet.Cells[row, 24].Value?.ToString();
                            _context.Nagruzka1s.Add(data);
                        }
                        _context.SaveChanges();
                    }
                }
            }
        }

    }
}
