using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProgrammingSchoolApp.Models;

public partial class ProgrammingSchoolContext : DbContext
{
    public ProgrammingSchoolContext()
    {
    }

    public ProgrammingSchoolContext(DbContextOptions<ProgrammingSchoolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Coursehistory> Coursehistories { get; set; }

    public virtual DbSet<Courselevel> Courselevels { get; set; }

    public virtual DbSet<Coursepricehistory> Coursepricehistories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Lessonprogress> Lessonprogresses { get; set; }

    public virtual DbSet<Programminglanguage> Programminglanguages { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("admins_pkey");

            entity.ToTable("admins");

            entity.HasIndex(e => e.Employeecode, "admins_employeecode_key").IsUnique();

            entity.Property(e => e.Userid)
                .ValueGeneratedNever()
                .HasColumnName("userid");
            entity.Property(e => e.Employeecode)
                .HasMaxLength(50)
                .HasColumnName("employeecode");
            entity.Property(e => e.Position)
                .HasMaxLength(100)
                .HasColumnName("position");

            entity.HasOne(d => d.User).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.Userid)
                .HasConstraintName("admins_userid_fkey");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("courses_pkey");

            entity.ToTable("courses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Languageid).HasColumnName("languageid");
            entity.Property(e => e.Levelid).HasColumnName("levelid");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Language).WithMany(p => p.Courses)
                .HasForeignKey(d => d.Languageid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("courses_languageid_fkey");

            entity.HasOne(d => d.Level).WithMany(p => p.Courses)
                .HasForeignKey(d => d.Levelid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("courses_levelid_fkey");
        });

        modelBuilder.Entity<Coursehistory>(entity =>
        {
            entity.HasKey(e => e.Historyid).HasName("coursehistory_pkey");

            entity.ToTable("coursehistory");

            entity.Property(e => e.Historyid).HasColumnName("historyid");
            entity.Property(e => e.Changedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("changedat");
            entity.Property(e => e.Changedescription).HasColumnName("changedescription");
            entity.Property(e => e.Courseid).HasColumnName("courseid");
        });

        modelBuilder.Entity<Courselevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("courselevels_pkey");

            entity.ToTable("courselevels");

            entity.HasIndex(e => e.Name, "courselevels_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Coursepricehistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("coursepricehistory_pkey");

            entity.ToTable("coursepricehistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adminid).HasColumnName("adminid");
            entity.Property(e => e.Changedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("changedat");
            entity.Property(e => e.Courseid).HasColumnName("courseid");
            entity.Property(e => e.Newprice)
                .HasPrecision(10, 2)
                .HasColumnName("newprice");
            entity.Property(e => e.Oldprice)
                .HasPrecision(10, 2)
                .HasColumnName("oldprice");

            entity.HasOne(d => d.Admin).WithMany(p => p.Coursepricehistories)
                .HasForeignKey(d => d.Adminid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("coursepricehistory_adminid_fkey");

            entity.HasOne(d => d.Course).WithMany(p => p.Coursepricehistories)
                .HasForeignKey(d => d.Courseid)
                .HasConstraintName("coursepricehistory_courseid_fkey");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.Property(e => e.Userid)
                .ValueGeneratedNever()
                .HasColumnName("userid");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.Userid)
                .HasConstraintName("customers_userid_fkey");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("enrollments_pkey");

            entity.ToTable("enrollments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Courseid).HasColumnName("courseid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Enrolledat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("enrolledat");

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.Courseid)
                .HasConstraintName("enrollments_courseid_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("enrollments_customerid_fkey");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lessons_pkey");

            entity.ToTable("lessons");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Courseid).HasColumnName("courseid");
            entity.Property(e => e.Ordernumber).HasColumnName("ordernumber");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.Videourl)
                .HasMaxLength(500)
                .HasColumnName("videourl");

            entity.HasOne(d => d.Course).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.Courseid)
                .HasConstraintName("lessons_courseid_fkey");
        });

        modelBuilder.Entity<Lessonprogress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lessonprogress_pkey");

            entity.ToTable("lessonprogress");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Completedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completedat");
            entity.Property(e => e.Enrollmentid).HasColumnName("enrollmentid");
            entity.Property(e => e.Iscompleted)
                .HasDefaultValue(false)
                .HasColumnName("iscompleted");
            entity.Property(e => e.Lessonid).HasColumnName("lessonid");

            entity.HasOne(d => d.Enrollment).WithMany(p => p.Lessonprogresses)
                .HasForeignKey(d => d.Enrollmentid)
                .HasConstraintName("lessonprogress_enrollmentid_fkey");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Lessonprogresses)
                .HasForeignKey(d => d.Lessonid)
                .HasConstraintName("lessonprogress_lessonid_fkey");
        });

        modelBuilder.Entity<Programminglanguage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("programminglanguages_pkey");

            entity.ToTable("programminglanguages");

            entity.HasIndex(e => e.Name, "programminglanguages_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
