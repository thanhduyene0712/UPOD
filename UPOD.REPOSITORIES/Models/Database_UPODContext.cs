using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UPOD.REPOSITORIES.Models
{
    public partial class Database_UPODContext : DbContext
    {
        public Database_UPODContext()
        {
        }

        public Database_UPODContext(DbContextOptions<Database_UPODContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Agency> Agencies { get; set; } = null!;
        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<Contract> Contracts { get; set; } = null!;
        public virtual DbSet<ContractService> ContractServices { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Device> Devices { get; set; } = null!;
        public virtual DbSet<DeviceType> DeviceTypes { get; set; } = null!;
        public virtual DbSet<Guideline> Guidelines { get; set; } = null!;
        public virtual DbSet<Request> Requests { get; set; } = null!;
        public virtual DbSet<RequestHistory> RequestHistories { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;
        public virtual DbSet<Technician> Technicians { get; set; } = null!;
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost,1433;Initial Catalog=Database_UPOD;User ID=sa;Password=THANHDUYEN07121999;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Username).HasMaxLength(250);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("AccountRole");
            });

            modelBuilder.Entity<Agency>(entity =>
            {
                entity.ToTable("Agency");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AgencyName).HasMaxLength(250);

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ManagerName).HasMaxLength(250);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Agencies)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK_Agency_Area");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Agencies)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("AgencyAccount");
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AreaName).HasMaxLength(250);

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contract");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.ContractName).HasMaxLength(200);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.PunishmentForCustomer).HasMaxLength(200);

                entity.Property(e => e.PunishmentForIt)
                    .HasMaxLength(200)
                    .HasColumnName("PunishmentForIT");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TimeCommit).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("ContractCompany");
            });

            modelBuilder.Entity<ContractService>(entity =>
            {
                entity.ToTable("ContractService");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.ContractServices)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("ContractServiceITSupporterContract");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ContractServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("ContractServiceITSupporterServiceItSupport");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Company_Account");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("Device");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DeviceAccount)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceName).HasMaxLength(250);

                entity.Property(e => e.DevicePassword)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GuarantyEndDate).HasColumnType("datetime");

                entity.Property(e => e.GuarantyStartDate).HasColumnType("datetime");

                entity.Property(e => e.Ip)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Other).HasMaxLength(100);

                entity.Property(e => e.SettingDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Agency)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.AgencyId)
                    .HasConstraintName("FK_Device_Agency");

                entity.HasOne(d => d.DeviceType)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.DeviceTypeId)
                    .HasConstraintName("DeviceDeviceType");
            });

            modelBuilder.Entity<DeviceType>(entity =>
            {
                entity.ToTable("DeviceType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DeviceTypeName).HasMaxLength(250);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.DeviceTypes)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("DeviceTypeServiceITSupport");
            });

            modelBuilder.Entity<Guideline>(entity =>
            {
                entity.ToTable("Guideline");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Guideline1).HasColumnName("Guideline");

                entity.Property(e => e.GuidelineName).HasMaxLength(255);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Guidelines)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_Guideline_Service");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("Request");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CurrentTechnicianId).HasColumnName("CurrentTechnician_Id");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.ExceptionSource).HasMaxLength(255);

                entity.Property(e => e.Img)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Agency)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.AgencyId)
                    .HasConstraintName("FK_Request_Agency");

                entity.HasOne(d => d.CurrentTechnician)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.CurrentTechnicianId)
                    .HasConstraintName("FK_Request_ITSupporter");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Request_Company");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_Request_Service");
            });

            modelBuilder.Entity<RequestHistory>(entity =>
            {
                entity.ToTable("RequestHistory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.PreStatus).HasColumnName("Pre_Status");

                entity.Property(e => e.PreTechnicianId).HasColumnName("Pre_TechnicianId");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.PreTechnician)
                    .WithMany(p => p.RequestHistories)
                    .HasForeignKey(d => d.PreTechnicianId)
                    .HasConstraintName("RequestHistoryITSupporter");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.RequestHistories)
                    .HasForeignKey(d => d.RequestId)
                    .HasConstraintName("RequestHistoryRequest");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ServiceName).HasMaxLength(250);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable("Skill");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Skills)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("SkillServiceITSupport");

                entity.HasOne(d => d.Technician)
                    .WithMany(p => p.Skills)
                    .HasForeignKey(d => d.TechnicianId)
                    .HasConstraintName("SkillITSupporter");
            });

            modelBuilder.Entity<Technician>(entity =>
            {
                entity.ToTable("Technician");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RatingAvg).HasColumnName("RatingAVG");

                entity.Property(e => e.TechnicianName).HasMaxLength(100);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Technicians)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("ITSupporterAccount");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Technicians)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("ITSupporterDepartment");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Solution).HasMaxLength(250);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("TicketDevice");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.RequestId)
                    .HasConstraintName("TicketRequest");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
