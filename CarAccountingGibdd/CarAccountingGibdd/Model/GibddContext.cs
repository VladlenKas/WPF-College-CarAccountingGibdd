using System;
using System.Collections.Generic;
using System.Net;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Model;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace CarAccountingGibdd.Model;

public partial class GibddContext : DbContext
{
    public GibddContext()
    {
    }

    public GibddContext(DbContextOptions<GibddContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> AllApplications { get; set; }

    public virtual DbSet<ApplicationStatus> ApplicationStatuses { get; set; }

    public virtual DbSet<Certificate> AllCertificates { get; set; }

    public virtual DbSet<Department> AllDepartments { get; set; }

    public virtual DbSet<Employee> AllEmployees { get; set; }

    public virtual DbSet<Inspection> AllInspections { get; set; }

    public virtual DbSet<InspectionStatus> InspectionStatuses { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<PhotosVehicle> PhotosVehicles { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Vehicle> AllVehicles { get; set; }

    public virtual DbSet<VehicleType> VehicleTypes { get; set; }

    public virtual DbSet<Violation> AllViolations { get; set; }

    public virtual DbSet<ViolationInspection> AllViolationInspections { get; set; }

    // Записи активные и с контекстом

    public IQueryable<Inspection> Inspections => AllInspections
        .Include(a => a.Application)
            .ThenInclude(o => o.Owner)
        .Include(a => a.Application)
            .ThenInclude(v => v.Vehicle)
            .ThenInclude(vt => vt.VehicleType)
        .Include(s => s.Status);

    public IQueryable<Application> Applications => AllApplications
        .Include(o => o.Owner)
        .Include(v => v.Vehicle)
            .ThenInclude(vt => vt.VehicleType)
        .Include(ins => ins.Inspections)
            .ThenInclude(i => i.Inspector)
        .Include(ins => ins.Inspections)
            .ThenInclude(s => s.Status)
        .Include(a => a.ApplicationStatus)
        .Include(op => op.Operator)
        .Include(c => c.Certificates)
        .AsSplitQuery(); // для оптимизации

    public IQueryable<Employee> Employees => AllEmployees
        .Where(r => r.Deleted != 1)
        .Include(r => r.Post)
        .Include(r => r.Department);

    public IQueryable<Department> Departments => AllDepartments
        .Where(r => r.Deleted != 1)
        .Include(r => r.Employees)
            .ThenInclude(r => r.Post);

    public IQueryable<Violation> Violations => AllViolations
        .Where(r => r.Deleted != 1);

    public IQueryable<ViolationInspection> ViolationInspection => AllViolationInspections
        .Include(v => v.Violation)
        .Include(i => i.Inspection);

    public IQueryable<Vehicle> Vehicles => AllVehicles
        .Include(r => r.PhotosVehicles)
        .Include(r => r.VehicleType);

    public IQueryable<Certificate> Certificates => AllCertificates
        .Include(a => a.Application)
            .ThenInclude(o => o.Owner)
        .Include(a => a.Application)
            .ThenInclude(v => v.Vehicle)
            .ThenInclude(vt => vt.VehicleType)
        .Include(a => a.Application)
            .ThenInclude(ins => ins.Inspections)
                .ThenInclude(i => i.Inspector);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=gibdd", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PRIMARY");

            entity.ToTable("application");

            entity.HasIndex(e => e.ApplicationStatusId, "fk_application_status_idx");

            entity.HasIndex(e => e.OperatorId, "operator_id_idx");

            entity.HasIndex(e => e.OwnerId, "owner_id_idx");

            entity.HasIndex(e => e.VehicleId, "vehicle_id_idx");

            entity.Property(e => e.ApplicationId).HasColumnName("application_id");
            entity.Property(e => e.ApplicationStatusId).HasColumnName("application_status_id");
            entity.Property(e => e.DatetimeConfirm)
                .HasColumnType("datetime")
                .HasColumnName("datetime_confirm");
            entity.Property(e => e.DatetimeSupply)
                .HasColumnType("datetime")
                .HasColumnName("datetime_supply");            
            entity.Property(e => e.DatetimeAccept)
                .HasColumnType("datetime")
                .HasColumnName("datetime_accept");
            entity.Property(e => e.OperatorId).HasColumnName("operator_id");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.ApplicationStatus).WithMany(p => p.Applications)
                .HasForeignKey(d => d.ApplicationStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_application_status");

            entity.HasOne(d => d.Operator).WithMany(p => p.Applications)
                .HasForeignKey(d => d.OperatorId)
                .HasConstraintName("operator_id");

            entity.HasOne(d => d.Owner).WithMany(p => p.Applications)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("owner_fkk");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Applications)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vehicle_fkk");
        });

        modelBuilder.Entity<ApplicationStatus>(entity =>
        {
            entity.HasKey(e => e.ApplicationStatusId).HasName("PRIMARY");

            entity.ToTable("application_status");

            entity.Property(e => e.ApplicationStatusId).HasColumnName("application_status_id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.CertificateId).HasName("PRIMARY");

            entity.ToTable("certificate");

            entity.HasIndex(e => e.ApplicationId, "fk_certificate_application");

            entity.HasIndex(e => e.Number, "number_UNIQUE").IsUnique();

            entity.Property(e => e.CertificateId).HasColumnName("certificate_id");
            entity.Property(e => e.ApplicationId).HasColumnName("application_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IssueDate).HasColumnName("issue_date");
            entity.Property(e => e.Number)
                .HasMaxLength(10)
                .HasColumnName("number");

            entity.HasOne(d => d.Application).WithMany(p => p.Certificates)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_certificate_application");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PRIMARY");

            entity.ToTable("department");

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.Address)
                .HasMaxLength(120)
                .HasColumnName("address");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PRIMARY");

            entity.ToTable("employee");

            entity.HasIndex(e => e.DepartmentId, "department_id_idx");

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Passport, "passport_UNIQUE").IsUnique();

            entity.HasIndex(e => e.PostId, "position_id_idx");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.Firstname)
                .HasMaxLength(45)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(45)
                .HasColumnName("lastname");
            entity.Property(e => e.Email)
                .HasMaxLength(90)
                .HasColumnName("email");
            entity.Property(e => e.Passport)
                .HasMaxLength(10)
                .HasColumnName("passport");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(45)
                .HasColumnName("patronymic");
            entity.Property(e => e.PostId).HasColumnName("post_id");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("department_id");

            entity.HasOne(d => d.Post).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("post_id");
        });

        modelBuilder.Entity<Inspection>(entity =>
        {
            entity.HasKey(e => e.InspectionId).HasName("PRIMARY");

            entity.ToTable("inspection");

            entity.HasIndex(e => e.ApplicationId, "application_id_idx");

            entity.HasIndex(e => e.InspectorId, "employee_id_idx");

            entity.HasIndex(e => e.StatusId, "result_id_idx");

            entity.Property(e => e.InspectionId).HasColumnName("inspection_id");
            entity.Property(e => e.ApplicationId).HasColumnName("application_id");
            entity.Property(e => e.DatetimeCompleted)
                .HasColumnType("datetime")
                .HasColumnName("datetime_completed");
            entity.Property(e => e.InspectorId).HasColumnName("inspector_id");
            entity.Property(e => e.DatetimePlanned)
                .HasColumnType("datetime")
                .HasColumnName("datetime_planned");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Application).WithMany(p => p.Inspections)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("application_id");

            entity.HasOne(d => d.Inspector).WithMany(p => p.Inspections)
                .HasForeignKey(d => d.InspectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("inspector_id");

            entity.HasOne(d => d.Status).WithMany(p => p.Inspections)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("status_id");
        });

        modelBuilder.Entity<InspectionStatus>(entity =>
        {
            entity.HasKey(e => e.InspectionStatusId).HasName("PRIMARY");

            entity.ToTable("inspection_status");

            entity.Property(e => e.InspectionStatusId).HasColumnName("inspection_status_id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.OwnerId).HasName("PRIMARY");

            entity.ToTable("owner");

            entity.HasIndex(e => e.Passport, "passport_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Phone, "phone_UNIQUE").IsUnique();

            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Address)
                .HasMaxLength(120)
                .HasColumnName("address");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Email)
                .HasMaxLength(90)
                .HasColumnName("email");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.Firstname)
                .HasMaxLength(45)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(45)
                .HasColumnName("lastname");
            entity.Property(e => e.Passport)
                .HasMaxLength(10)
                .HasColumnName("passport");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(45)
                .HasColumnName("patronymic");
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<PhotosVehicle>(entity =>
        {
            entity.HasKey(e => e.PhotosVehicleId).HasName("PRIMARY");

            entity.ToTable("photos_vehicle");

            entity.HasIndex(e => e.VehicleId, "vehicle_id_idx");

            entity.Property(e => e.PhotosVehicleId).HasColumnName("photos_vehicle_id");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.PhotosVehicles)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vehicle_id");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PRIMARY");

            entity.ToTable("post");

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PRIMARY");

            entity.ToTable("vehicle");

            entity.HasIndex(e => e.VehicleTypeId, "vehicle_type_id_idx");

            entity.HasIndex(e => e.Vin, "vin_UNIQUE").IsUnique();

            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");
            entity.Property(e => e.Brand)
                .HasMaxLength(45)
                .HasColumnName("brand");
            entity.Property(e => e.Color)
                .HasMaxLength(45)
                .HasColumnName("color");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(6)
                .HasColumnName("license_plate");
            entity.Property(e => e.Model)
                .HasMaxLength(45)
                .HasColumnName("model");
            entity.Property(e => e.Used).HasColumnName("used");
            entity.Property(e => e.VehicleTypeId).HasColumnName("vehicle_type_id");
            entity.Property(e => e.Vin)
                .HasMaxLength(17)
                .HasColumnName("vin");
            entity.Property(e => e.Year)
                .HasColumnType("year")
                .HasColumnName("year");

            entity.HasOne(d => d.VehicleType).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.VehicleTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vehicle_type_id");
        });

        modelBuilder.Entity<VehicleType>(entity =>
        {
            entity.HasKey(e => e.VehicleTypeId).HasName("PRIMARY");

            entity.ToTable("vehicle_type");

            entity.Property(e => e.VehicleTypeId).HasColumnName("vehicle_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Violation>(entity =>
        {
            entity.HasKey(e => e.ViolationId).HasName("PRIMARY");

            entity.ToTable("violation");

            entity.Property(e => e.ViolationId).HasColumnName("violation_id");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.Description)
                .HasMaxLength(90)
                .HasColumnName("description");
        });

        modelBuilder.Entity<ViolationInspection>(entity =>
        {
            entity.HasKey(e => e.ViolationInspectionId).HasName("PRIMARY");

            entity.ToTable("violation_inspection");

            entity.HasIndex(e => e.InspectionId, "inspection_id_idx");

            entity.HasIndex(e => e.ViolationId, "violation_id_idx");

            entity.Property(e => e.ViolationInspectionId).HasColumnName("violation_inspection_id");
            entity.Property(e => e.InspectionId).HasColumnName("inspection_id");
            entity.Property(e => e.ViolationId).HasColumnName("violation_id");

            entity.HasOne(d => d.Inspection).WithMany(p => p.ViolationsInspections)
                .HasForeignKey(d => d.InspectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("inspection_id");

            entity.HasOne(d => d.Violation).WithMany(p => p.ViolationInspections)
                .HasForeignKey(d => d.ViolationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("violation_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
