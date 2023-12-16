using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using ExcelDataReader;
using System.Data;
using System;
using System.Text;
using System.Reflection;
using Npgsql;
using System.Diagnostics;

namespace DigitalAssistantApp.Pages.ImportPage
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IFormFile Upload { get; set; }

        public async Task OnPostAsync()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Upload.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }
            var fileInfo = new FileInfo(file);
            string importfilepath = "./wwwroot/import.csv";
            ExcelPrepare(fileInfo);
            XlsxToCsvConverter(fileInfo, importfilepath);
            var importfileinfo = new FileInfo(importfilepath);
            ImportFileToDatabase(importfileinfo);
            //fileInfo.Delete();
        }
        public async Task OnPostTestAsync()
        {
            using (var connection = new NpgsqlConnection("Server=172.20.7.9;Port=5432;Database=dad_test;User Id=superuser;Password=rootUSER"))
            {
                connection.Open();
                /*using (var command = new NpgsqlCommand())
                {
                    //command.Connection = connection;
                    //command.CommandText = @"copy nagruzka1 FROM 'C:\\Users\\Twelfth\\Source\\Repos\\RickIam\\DigitalAssistantApp\\DigitalAssistantApp\\wwwroot\\import.csv' (format CSV, HEADER, delimiter ';',null 'NULL', encoding 'UTF8')";
                    //command.ExecuteNonQuery(); 
                }*/
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
            }
        }
        private void ExcelPrepare(FileInfo fileInfo)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;         //noncommercial-��� ������������������ ��������
            using (ExcelPackage package = new ExcelPackage(fileInfo.FullName))
            {
                var worksheet = package.Workbook.Worksheets[0];
                worksheet.DeleteRow(1);
                int endrow = -1;
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
                for (int row = 2; row < endrow; row++)
                {
                    for (int col = 7; col <= 23; col++)
                    {
                        if (worksheet.Cells[row, col].Value != null)
                        {
                            worksheet.Cells[row, col].Value = worksheet.Cells[row, col].Value.ToString().Replace(",", ".");
                        }
                        else
                        {
                            worksheet.Cells[row, col].Value = 0;
                        }
                    }
                }
                package.Save();
            }
        }
        private void XlsxToCsvConverter(FileInfo fileInfo, string importfilepath)
        {
            FileStream stream = System.IO.File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet result = excelReader.AsDataSet();
            DataTable dataTable = result.Tables[0];
            StreamWriter csvWriter = new StreamWriter(importfilepath);
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (i != dataTable.Columns.Count - 1)
                    {
                        csvWriter.Write(row[i].ToString() + ";");
                    }
                    else
                    {
                        csvWriter.Write(row[i].ToString());
                    }
                }
                csvWriter.WriteLine();
            }
            csvWriter.Flush();
            csvWriter.Close();
            excelReader.Close();
        }
        private void ImportFileToDatabase(FileInfo importfilepath)
        {
            /*try
            {
                string psqlPath = "C:\\Program Files\\PostgreSQL\\16\\scripts\\runpsql.bat";
                string arguments = $"-h 172.20.7.9 -p 5432 -U superuser -W rootUSER -d dad_test -c \"COPY nagruzka1 FROM '{importfilepath.FullName}' WITH CSV HEADER DELIMITER ';' NULL 'NULL' ENCODING 'UTF8';\"";
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = psqlPath,
                        Arguments = arguments,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                //process.Start();
                //process.Close();
                //process.Dispose();
                //string connectionString = "Server=172.20.7.9;Port=5432;Database=dad_test;User Id=superuser;Password=rootUSER";
                //string copyCommand = $"\\copy nagruzka1 FROM '{importfilepath.FullName}' (format CSV, HEADER, delimiter ';', null 'NULL', encoding 'UTF8')";
                // ������ ������� psql � �������� \copy
                /*ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = "C:\\Program Files\\PostgreSQL\\16\\scripts\\runpsql.bat",
                    Arguments = $"-h 172.20.7.9 -p 5432 -d dad_test -U superuser -P rootUSER -c + \"{copyCommand}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };
                using (Process process = new Process { StartInfo = processInfo })
                {
                    //process.Start();
                    //string output = process.StandardOutput.ReadToEnd();
                    //string error = process.StandardError.ReadToEnd();
                    //process.WaitForExit();
                    //Console.WriteLine("Output: " + output);
                    //Console.WriteLine("Error: " + error);

                    //Console.WriteLine("�������� copy ��������� �������.");
                }*/
            /*}
            catch (Exception ex)
            {
                Console.WriteLine($"��������� ������: {ex.Message}");
            }*/



        }
 
    }
}
