using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace lab2._1.Models;

public partial class ITIDbContext : DbContext
{
    public ITIDbContext()
    {
    }

    public ITIDbContext(DbContextOptions<ITIDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<DailyTransaction> DailyTransactions { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DisplayInstructor> DisplayInstructors { get; set; }

    public virtual DbSet<DisplayManager> DisplayManagers { get; set; }

    public virtual DbSet<InsCourse> InsCourses { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<LatestTransaction> LatestTransactions { get; set; }

    public virtual DbSet<StudCourse> StudCourses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentDisplay> StudentDisplays { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<V1> V1s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-DC6PHRTK\\SQLEXPRESS;Initial Catalog=ITI;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CrsId);

            entity.ToTable("Course", "fr");

            entity.Property(e => e.CrsId)
                .ValueGeneratedNever()
                .HasColumnName("Crs_Id");
            entity.Property(e => e.CrsDuration).HasColumnName("Crs_Duration");
            entity.Property(e => e.CrsName)
                .HasMaxLength(50)
                .HasColumnName("Crs_Name");
            entity.Property(e => e.TopId).HasColumnName("Top_Id");

            entity.HasOne(d => d.Top).WithMany(p => p.Courses)
                .HasForeignKey(d => d.TopId)
                .HasConstraintName("FK_Course_Topic");
        });

        modelBuilder.Entity<DailyTransaction>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Daily_tr__B9BE370F3909B246");

            entity.ToTable("Daily_transactions");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DeptId);

            entity.ToTable("Department");

            entity.Property(e => e.DeptId)
                .ValueGeneratedNever()
                .HasColumnName("Dept_Id");
            entity.Property(e => e.DeptDesc)
                .HasMaxLength(100)
                .HasColumnName("Dept_Desc");
            entity.Property(e => e.DeptLocation)
                .HasMaxLength(50)
                .HasColumnName("Dept_Location");
            entity.Property(e => e.DeptManager).HasColumnName("Dept_Manager");
            entity.Property(e => e.DeptName)
                .HasMaxLength(50)
                .HasColumnName("Dept_Name");
            entity.Property(e => e.ManagerHiredate).HasColumnName("Manager_hiredate");

            entity.HasOne(d => d.DeptManagerNavigation).WithMany(p => p.Departments)
                .HasForeignKey(d => d.DeptManager)
                .HasConstraintName("FK_Department_Instructor");
        });

        modelBuilder.Entity<DisplayInstructor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("display_instructor");

            entity.Property(e => e.DeptName)
                .HasMaxLength(50)
                .HasColumnName("Dept_Name");
            entity.Property(e => e.InsName)
                .HasMaxLength(50)
                .HasColumnName("Ins_Name");
        });

        modelBuilder.Entity<DisplayManager>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("display_managers");

            entity.Property(e => e.CrsName)
                .HasMaxLength(50)
                .HasColumnName("Crs_Name");
            entity.Property(e => e.Manager).HasMaxLength(50);
        });

        modelBuilder.Entity<InsCourse>(entity =>
        {
            entity.HasKey(e => new { e.InsId, e.CrsId });

            entity.ToTable("Ins_Course");

            entity.Property(e => e.InsId).HasColumnName("Ins_Id");
            entity.Property(e => e.CrsId).HasColumnName("Crs_Id");
            entity.Property(e => e.Evaluation).HasMaxLength(50);

            entity.HasOne(d => d.Crs).WithMany(p => p.InsCourses)
                .HasForeignKey(d => d.CrsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ins_Course_Course");

            entity.HasOne(d => d.Ins).WithMany(p => p.InsCourses)
                .HasForeignKey(d => d.InsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ins_Course_Instructor");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.InsId);

            entity.ToTable("Instructor");

            entity.Property(e => e.InsId)
                .ValueGeneratedNever()
                .HasColumnName("Ins_Id");
            entity.Property(e => e.DeptId).HasColumnName("Dept_Id");
            entity.Property(e => e.InsDegree)
                .HasMaxLength(50)
                .HasColumnName("Ins_Degree");
            entity.Property(e => e.InsName)
                .HasMaxLength(50)
                .HasColumnName("Ins_Name");
            entity.Property(e => e.Salary).HasColumnType("money");

            entity.HasOne(d => d.Dept).WithMany(p => p.Instructors)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK_Instructor_Department");
        });

        modelBuilder.Entity<LatestTransaction>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Latest_t__B9BE370F87B9C1D0");

            entity.ToTable("Latest_transactions");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
        });

        modelBuilder.Entity<StudCourse>(entity =>
        {
            entity.HasKey(e => new { e.CrsId, e.StId });

            entity.ToTable("Stud_Course");

            entity.Property(e => e.CrsId).HasColumnName("Crs_Id");
            entity.Property(e => e.StId).HasColumnName("St_Id");

            entity.HasOne(d => d.Crs).WithMany(p => p.StudCourses)
                .HasForeignKey(d => d.CrsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stud_Course_Course");

            entity.HasOne(d => d.St).WithMany(p => p.StudCourses)
                .HasForeignKey(d => d.StId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stud_Course_Student");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StId);

            entity.ToTable("Student", "fr");

            entity.Property(e => e.StId)
                .ValueGeneratedNever()
                .HasColumnName("St_Id");
            entity.Property(e => e.DeptId).HasColumnName("Dept_Id");
            entity.Property(e => e.StAddress)
                .HasMaxLength(100)
                .HasColumnName("St_Address");
            entity.Property(e => e.StAge).HasColumnName("St_Age");
            entity.Property(e => e.StFname)
                .HasMaxLength(50)
                .HasColumnName("St_Fname");
            entity.Property(e => e.StLname)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("St_Lname");
            entity.Property(e => e.StSuper).HasColumnName("St_super");

            entity.HasOne(d => d.Dept).WithMany(p => p.Students)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK_Student_Department");

            entity.HasOne(d => d.StSuperNavigation).WithMany(p => p.InverseStSuperNavigation)
                .HasForeignKey(d => d.StSuper)
                .HasConstraintName("FK_Student_Student");
        });

        modelBuilder.Entity<StudentDisplay>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("student_display");

            entity.Property(e => e.CrsName)
                .HasMaxLength(50)
                .HasColumnName("Crs_Name");
            entity.Property(e => e.FullName).HasMaxLength(61);
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.TopId);

            entity.ToTable("Topic");

            entity.Property(e => e.TopId)
                .ValueGeneratedNever()
                .HasColumnName("Top_Id");
            entity.Property(e => e.TopName)
                .HasMaxLength(50)
                .HasColumnName("Top_Name");
        });

        modelBuilder.Entity<V1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V1");

            entity.Property(e => e.DeptId).HasColumnName("Dept_Id");
            entity.Property(e => e.StAddress)
                .HasMaxLength(100)
                .HasColumnName("St_Address");
            entity.Property(e => e.StAge).HasColumnName("St_Age");
            entity.Property(e => e.StFname)
                .HasMaxLength(50)
                .HasColumnName("St_Fname");
            entity.Property(e => e.StId).HasColumnName("St_Id");
            entity.Property(e => e.StLname)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("St_Lname");
            entity.Property(e => e.StSuper).HasColumnName("St_super");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
