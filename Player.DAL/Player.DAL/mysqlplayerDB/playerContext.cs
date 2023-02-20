using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class playerContext : DbContext
    {
        public playerContext()
        {
        }

        public playerContext(DbContextOptions<playerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AddressZone> AddressZones { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<AreaLocalize> AreaLocalizes { get; set; }
        public virtual DbSet<BuddyRequest> BuddyRequests { get; set; }
        public virtual DbSet<CommonUserDevice> CommonUserDevices { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CountryLocalize> CountryLocalizes { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<PlayerBlock> PlayerBlocks { get; set; }
        public virtual DbSet<PlayerEnergyPoint> PlayerEnergyPoints { get; set; }
        public virtual DbSet<PlayerInfo> PlayerInfos { get; set; }
        public virtual DbSet<PlayerReport> PlayerReports { get; set; }
        public virtual DbSet<PlayerSport> PlayerSports { get; set; }
        public virtual DbSet<PlayerZone> PlayerZones { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<SportLocalize> SportLocalizes { get; set; }
        public virtual DbSet<Zone> Zones { get; set; }
        public virtual DbSet<ZoneLocalize> ZoneLocalizes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=c4s-mysql-dev.cec7g9j6m57h.us-east-1.rds.amazonaws.com;port=3306;database=player;user id=admin;password=0XQjoDAO4QV164IvR1cT", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<AddressZone>(entity =>
            {
                entity.ToTable("Address_Zone");

                entity.HasIndex(e => e.PlayerId, "FK_PlayerId_AddressZone_idx");

                entity.HasIndex(e => e.ZoneId, "FK_ZoneId_AddresZone_idx");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(650);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.CreatedOn)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.Floor).HasMaxLength(45);

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.IsHomeAddress).HasColumnType("bit(1)");

                entity.Property(e => e.PlayerId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Street).HasMaxLength(245);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UpdatedOn)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.AddressZones)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlayerId_AddressZone");

                entity.HasOne(d => d.Zone)
                    .WithMany(p => p.AddressZones)
                    .HasForeignKey(d => d.ZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZoneId_AddresZone");
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area");

                entity.HasIndex(e => e.CountryId, "FK_CountryId_Area_idx");

                entity.Property(e => e.CountryId)
                    .IsRequired()
                    .HasMaxLength(38)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.CreatedOn)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UpdatedOn)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Areas)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CountryId_Area");
            });

            modelBuilder.Entity<AreaLocalize>(entity =>
            {
                entity.ToTable("Area_Localize");

                entity.HasIndex(e => e.AreaId, "FK_AreaId_AreaLocalize_idx");

                entity.HasIndex(e => e.LanguageId, "FK_LanguageId_AreaLocalize_idx");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.CreatedOn)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UpdatedOn)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.AreaLocalizes)
                    .HasForeignKey(d => d.AreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AreaId_AreaLocalize");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.AreaLocalizes)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageId_AreaLocalize");
            });

            modelBuilder.Entity<BuddyRequest>(entity =>
            {
                entity.ToTable("buddyRequest");

                entity.HasIndex(e => e.CreatedById, "fk-sender_idx");

                entity.HasIndex(e => e.SportId, "fk_player-sport_idx");

                entity.HasIndex(e => e.PlayerId, "fk_reciever_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(6)
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.CreatedById)
                    .IsRequired()
                    .HasColumnName("createdById");

                entity.Property(e => e.IsConnectionOnly).HasDefaultValueSql("'0'");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.PlayerId)
                    .IsRequired()
                    .HasColumnName("playerId");

                entity.Property(e => e.SportId).HasColumnName("sportId");

                entity.Property(e => e.StatusId).HasColumnName("statusId");

                entity.Property(e => e.UpdatedAt)
                    .HasMaxLength(6)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updatedAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.BuddyRequestCreatedBies)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk-sender");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.BuddyRequestPlayers)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_reciever");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.BuddyRequests)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("fk_player-sport");
            });

            modelBuilder.Entity<CommonUserDevice>(entity =>
            {
                entity.ToTable("Common_UserDevice");

                entity.HasIndex(e => e.CommonUserId, "fk-playerid_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AuthCreationDate).HasColumnType("datetime");

                entity.Property(e => e.AuthExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.AuthIp)
                    .HasMaxLength(100)
                    .HasColumnName("AuthIP")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.AuthToken)
                    .HasMaxLength(250)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.CommonUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Common_UserID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DeviceEmail)
                    .HasMaxLength(150)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DeviceImei)
                    .HasMaxLength(150)
                    .HasColumnName("DeviceIMEI")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DeviceMobileNumber)
                    .HasMaxLength(50)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DeviceName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DeviceOsversion)
                    .HasMaxLength(150)
                    .HasColumnName("DeviceOSVersion")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DeviceToken)
                    .HasMaxLength(500)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DeviceType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.EnableNotification).HasColumnType("bit(1)");

                entity.Property(e => e.IsDeleted).HasColumnType("bit(1)");

                entity.Property(e => e.IsGoogleSupport).HasColumnType("bit(1)");

                entity.Property(e => e.IsLoggedIn).HasColumnType("bit(1)");

                entity.Property(e => e.Lang)
                    .HasMaxLength(50)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.LastActiveDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.CommonUser)
                    .WithMany(p => p.CommonUserDevices)
                    .HasForeignKey(d => d.CommonUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk-playerid");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.Id)
                    .HasMaxLength(38)
                    .IsFixedLength(true);

                entity.Property(e => e.Code)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CurrencyCode).HasMaxLength(45);

                entity.Property(e => e.CurrencyName).HasMaxLength(45);

                entity.Property(e => e.Ibanlength).HasColumnName("IBANLength");

                entity.Property(e => e.Ibanprefix)
                    .HasMaxLength(128)
                    .HasColumnName("IBANPrefix")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.IconUrl)
                    .HasMaxLength(150)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.IsArabic).HasColumnType("bit(1)");

                entity.Property(e => e.IsPercentageTax)
                    .HasColumnType("bit(1)")
                    .HasColumnName("IsPercentageTAX")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.IsPercentageVat)
                    .HasColumnType("bit(1)")
                    .HasColumnName("IsPercentageVAT")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.NameAr)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.NameEn)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.PhoneCode)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.Tax).HasPrecision(18, 2);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.Vat)
                    .HasPrecision(18, 2)
                    .HasColumnName("VAT");
            });

            modelBuilder.Entity<CountryLocalize>(entity =>
            {
                entity.ToTable("Country_Localize");

                entity.HasIndex(e => e.CountryId, "FK_CountryId_CountryLocalize_idx");

                entity.HasIndex(e => e.LanguageId, "FK_LanguageId_CountryLocalize_idx");

                entity.Property(e => e.Code).HasMaxLength(128);

                entity.Property(e => e.CountryId)
                    .IsRequired()
                    .HasMaxLength(38)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(255)
                    .UseCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CurrencyCode).HasMaxLength(45);

                entity.Property(e => e.CurrencyName).HasMaxLength(45);

                entity.Property(e => e.Ibanprefix)
                    .HasMaxLength(128)
                    .HasColumnName("IBANPrefix")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(255)
                    .UseCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .UseCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.PhoneCode)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CountryLocalizes)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CountryId_CountryLocalize");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.CountryLocalizes)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageId_CountryLocalize");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("language");

                entity.UseCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IconUrl)
                    .HasMaxLength(355)
                    .HasColumnName("iconUrl");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("log");

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.RequestUrl).HasMaxLength(150);

                entity.Property(e => e.UpdatedAt)
                    .HasMaxLength(6)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.NotificationId).HasColumnName("NotificationID");

                entity.Property(e => e.AppearDate).HasColumnType("datetime");

                entity.Property(e => e.Body)
                    .HasMaxLength(500)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DeletionDate).HasColumnType("datetime");

                entity.Property(e => e.Icon)
                    .HasMaxLength(500)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.IsDeleted).HasColumnType("bit(1)");

                entity.Property(e => e.IsSeen).HasColumnType("bit(1)");

                entity.Property(e => e.IsSent).HasColumnType("bit(1)");

                entity.Property(e => e.NotificationTypeId).HasColumnName("NotificationTypeID");

                entity.Property(e => e.ObjectId).HasColumnName("ObjectID");

                entity.Property(e => e.RecipientId)
                    .HasMaxLength(200)
                    .HasColumnName("RecipientID");

                entity.Property(e => e.SeenDate).HasColumnType("datetime");

                entity.Property(e => e.SenderId)
                    .HasMaxLength(200)
                    .HasColumnName("SenderID");

                entity.Property(e => e.TargetPath)
                    .HasColumnType("text")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.ToTable("NotificationType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Message)
                    .HasMaxLength(500)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");
            });

            modelBuilder.Entity<PlayerBlock>(entity =>
            {
                entity.ToTable("playerBlock");

                entity.HasIndex(e => e.PlayerId, "fk-reciever_idx");

                entity.HasIndex(e => e.CreatedById, "fk-sender_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(6)
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.CreatedById)
                    .IsRequired()
                    .HasColumnName("createdById");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.PlayerId)
                    .IsRequired()
                    .HasColumnName("playerId");

                entity.Property(e => e.UpdatedAt)
                    .HasMaxLength(6)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updatedAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.PlayerBlockCreatedBies)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk-sender-block");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerBlockPlayers)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk-reciever-block");
            });

            modelBuilder.Entity<PlayerEnergyPoint>(entity =>
            {
                entity.ToTable("playerEnergyPoint");

                entity.HasIndex(e => e.PlayerId, "FK_PlayerId_PlayerEnergyPoint_idx");

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.PlayerId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProviderId).HasMaxLength(45);

                entity.Property(e => e.ProviderImageUrl).HasMaxLength(900);

                entity.Property(e => e.ProviderName).HasMaxLength(450);

                entity.Property(e => e.ProviderType)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.UpdatedAt)
                    .HasMaxLength(6)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerEnergyPoints)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlayerId_PlayerEnergyPoint");
            });

            modelBuilder.Entity<PlayerInfo>(entity =>
            {
                entity.ToTable("playerInfo");

                entity.HasIndex(e => e.Id, "Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.CountryId, "fk-country-player_idx");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BirthDate)
                    .HasColumnType("datetime")
                    .HasColumnName("birthDate");

                entity.Property(e => e.CountryId)
                    .HasMaxLength(38)
                    .HasColumnName("countryId")
                    .IsFixedLength(true);

                entity.Property(e => e.CoverUrl)
                    .HasMaxLength(250)
                    .HasColumnName("coverUrl");

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(6)
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("firstName")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasDefaultValueSql("'2'");

                entity.Property(e => e.Height)
                    .HasPrecision(8, 2)
                    .HasColumnName("height");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(250)
                    .HasColumnName("imageUrl");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasColumnName("isDeleted")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("lastName")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.Location)
                    .HasMaxLength(45)
                    .HasColumnName("location");

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("mobileNumber");

                entity.Property(e => e.Provider)
                    .HasMaxLength(45)
                    .HasColumnName("provider");

                entity.Property(e => e.TotalPoints)
                    .HasColumnName("totalPoints")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.UpdatedAt)
                    .HasMaxLength(6)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updatedAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.Weight)
                    .HasPrecision(8, 2)
                    .HasColumnName("weight");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.PlayerInfos)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_CountryId_Player");
            });

            modelBuilder.Entity<PlayerReport>(entity =>
            {
                entity.ToTable("player_report");

                entity.HasIndex(e => e.PlayerId, "FK_PlayerId_Report_idx");

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.PlayerId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerReports)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlayerId_Report");
            });

            modelBuilder.Entity<PlayerSport>(entity =>
            {
                entity.ToTable("playerSport");

                entity.HasIndex(e => e.PlayerId, "playersport_player_idx");

                entity.HasIndex(e => e.SportId, "playersport_sport_idx");

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(6)
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.PlayerId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("playerId");

                entity.Property(e => e.SportId).HasColumnName("sportId");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerSports)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("playersport_player");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.PlayerSports)
                    .HasForeignKey(d => d.SportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("playersport_sport");
            });

            modelBuilder.Entity<PlayerZone>(entity =>
            {
                entity.ToTable("Player_Zone");

                entity.HasIndex(e => e.PlayerId, "FK_PlayerId_PlayerZone_idx");

                entity.HasIndex(e => e.ZoneId, "FK_ZoneId_PlayerZone_idx");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.CreatedOn)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.PlayerId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Tax).HasPrecision(18, 2);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UpdatedOn)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerZones)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlayerId_PlayerZone");

                entity.HasOne(d => d.Zone)
                    .WithMany(p => p.PlayerZones)
                    .HasForeignKey(d => d.ZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZoneId_PlayerZone");
            });

            modelBuilder.Entity<Sport>(entity =>
            {
                entity.ToTable("sport");

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(6)
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.IconUrl)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("iconUrl");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.ModifiedBy).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UpdatedAt)
                    .HasMaxLength(6)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updatedAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            });

            modelBuilder.Entity<SportLocalize>(entity =>
            {
                entity.ToTable("sport_localize");

                entity.UseCollation("utf8mb4_unicode_ci");

                entity.HasIndex(e => e.LanguageId, "fk_sport_Localize_lang_idx");

                entity.HasIndex(e => e.SportId, "fk_sport_Localize_sport__idx");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.SportLocalizes)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sport_Localize_lang");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.SportLocalizes)
                    .HasForeignKey(d => d.SportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sport_Localize_sport");
            });

            modelBuilder.Entity<Zone>(entity =>
            {
                entity.ToTable("Zone");

                entity.HasIndex(e => e.AreaId, "FK_AreaId_Zone_idx");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.CreatedOn)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UpdatedOn)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Zones)
                    .HasForeignKey(d => d.AreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AreaId_Zone");
            });

            modelBuilder.Entity<ZoneLocalize>(entity =>
            {
                entity.ToTable("Zone_Localize");

                entity.HasIndex(e => e.LanguageId, "FK_LanguageId_ZoneLocalize_idx");

                entity.HasIndex(e => e.ZoneId, "FK_ZoneId_ZoneLocalize_idx");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.CreatedOn)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(128)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UpdatedOn)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.ZoneLocalizes)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageId_ZoneLocalize");

                entity.HasOne(d => d.Zone)
                    .WithMany(p => p.ZoneLocalizes)
                    .HasForeignKey(d => d.ZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZoneId_ZoneLocalize");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
