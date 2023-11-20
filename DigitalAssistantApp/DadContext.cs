using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssistantApp;

public partial class DadContext : DbContext
{
    public DadContext()
    {
    }

    public DadContext(DbContextOptions<DadContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EducPlan>? EducPlans { get; set; }

    public virtual DbSet<Faculty>? Faculties { get; set; }

    public virtual DbSet<PersonalLoad>? PersonalLoads { get; set; }

    public virtual DbSet<Speciality>? Specialities { get; set; }

    public virtual DbSet<Stream>? Streams { get; set; }

    public virtual DbSet<Subject>? Subjects { get; set; }

    public virtual DbSet<Teacher>? Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=172.20.7.9;Port=5432;Database=DAD;Username=superuser;Password=rootUSER");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EducPlan>(entity =>
        {
            entity.HasKey(e => e.EducPlanId).HasName("educ_plan_pkey");

            entity.ToTable("educ_plan");

            entity.Property(e => e.EducPlanId).HasColumnName("educ_plan_id");
            entity.Property(e => e.Att)
                .HasMaxLength(255)
                .HasColumnName("att");
            entity.Property(e => e.AudSrs).HasColumnName("aud_srs");
            entity.Property(e => e.Dept)
                .HasMaxLength(255)
                .HasColumnName("dept");
            entity.Property(e => e.H).HasColumnName("h");
            entity.Property(e => e.Ha).HasColumnName("ha");
            entity.Property(e => e.Hat).HasColumnName("hat");
            entity.Property(e => e.Hkr).HasColumnName("hkr");
            entity.Property(e => e.Hpr).HasColumnName("hpr");
            entity.Property(e => e.LabWorkCount).HasColumnName("lab_work_count");
            entity.Property(e => e.LectionsCount).HasColumnName("lections_count");
            entity.Property(e => e.PractiseCount).HasColumnName("practise_count");
            entity.Property(e => e.Season)
                .HasMaxLength(255)
                .HasColumnName("season");
            entity.Property(e => e.Semester).HasColumnName("semester");
            entity.Property(e => e.SpecId)
                .HasMaxLength(255)
                .HasColumnName("spec_id");
            entity.Property(e => e.StreamId).HasColumnName("stream_id");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.VarRasch).HasColumnName("var_rasch");
            entity.Property(e => e.Zet).HasColumnName("zet");

            entity.HasOne(d => d.Spec).WithMany(p => p.EducPlans)
                .HasForeignKey(d => d.SpecId)
                .HasConstraintName("educ_plan_spec_id_fkey");

            entity.HasOne(d => d.Stream).WithMany(p => p.EducPlans)
                .HasForeignKey(d => d.StreamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("educ_plan_stream_id_fkey");

            entity.HasOne(d => d.Subject).WithMany(p => p.EducPlans)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("educ_plan_subject_id_fkey");
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasKey(e => e.FacultyId).HasName("faculty_pkey");

            entity.ToTable("faculty");

            entity.Property(e => e.FacultyId).HasColumnName("faculty_id");
            entity.Property(e => e.FacultyName)
                .HasMaxLength(10)
                .HasColumnName("faculty_name");
        });

        modelBuilder.Entity<PersonalLoad>(entity =>
        {
            entity.HasKey(e => e.PersonalLoadId).HasName("personal_load_pkey");

            entity.ToTable("personal_load");

            entity.Property(e => e.PersonalLoadId)
                .ValueGeneratedOnAdd()
                .HasColumnName("personal_load_id");

            entity.HasOne(d => d.PersonalLoadNavigation).WithOne(p => p.PersonalLoad)
                .HasForeignKey<PersonalLoad>(d => d.PersonalLoadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("educ_plan_id");

            entity.HasOne(d => d.PersonalLoad1).WithOne(p => p.PersonalLoad)
                .HasForeignKey<PersonalLoad>(d => d.PersonalLoadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("teacher_id");
        });

        modelBuilder.Entity<Speciality>(entity =>
        {
            entity.HasKey(e => e.SpecId).HasName("speciality_pkey");

            entity.ToTable("speciality");

            entity.Property(e => e.SpecId)
                .HasMaxLength(255)
                .HasColumnName("spec_id");
            entity.Property(e => e.FacultyId).HasColumnName("faculty_id");

            entity.HasOne(d => d.Faculty).WithMany(p => p.Specialities)
                .HasForeignKey(d => d.FacultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("speciality_faculty_id_fkey");
        });

        modelBuilder.Entity<Stream>(entity =>
        {
            entity.HasKey(e => e.StreamId).HasName("stream_pkey");

            entity.ToTable("stream");

            entity.Property(e => e.StreamId).HasColumnName("stream_id");
            entity.Property(e => e.FOb)
                .HasMaxLength(5)
                .HasColumnName("f_ob");
            entity.Property(e => e.Group).HasColumnName("group");
            entity.Property(e => e.Semester).HasColumnName("semester");
            entity.Property(e => e.StudCount).HasColumnName("stud_count");
            entity.Property(e => e.StudForeign).HasColumnName("stud_foreign");
            entity.Property(e => e.StudVb).HasColumnName("stud_vb");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("subject_pkey");

            entity.ToTable("subject");

            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(255)
                .HasColumnName("subject_name");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("teacher_pkey");

            entity.ToTable("teacher");

            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.Department)
                .HasMaxLength(255)
                .HasColumnName("department");
            entity.Property(e => e.Firstname)
                .HasMaxLength(255)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .HasColumnName("lastname");
            entity.Property(e => e.PatronymicName)
                .HasMaxLength(255)
                .HasColumnName("patronymic_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
