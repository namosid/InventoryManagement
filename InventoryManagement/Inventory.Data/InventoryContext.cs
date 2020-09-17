using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Inventory.Data
{
    public partial class InventoryContext : DbContext
    {
        public InventoryContext()
        {
        }

        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(40);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.FirstName).HasMaxLength(40);

                entity.Property(e => e.LastName).HasMaxLength(40);

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsFixedLength();
            });

            modelBuilder.Query<SpLogin>();
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public void AddUser()
        {
            var param = new SqlParameter[] {
                new SqlParameter() {
                    ParameterName = "@pLogin",
                    SqlDbType =  System.Data.SqlDbType.NVarChar,
                    Size = 100,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = "LoginName"
                },
                new SqlParameter() {
                    ParameterName = "@pPassword",
                    SqlDbType =  System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = "Password"
                },
                new SqlParameter() {
                    ParameterName = "@pFirstName",
                    SqlDbType =  System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = "Password"
                },
                new SqlParameter() {
                    ParameterName = "@pLastName",
                    SqlDbType =  System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = "Password"
                }};
            
            this.User.FromSqlRaw("[dbo].[uspLogin] @pLogin, @pPassword, @pFirstName,@pLastName", param).ToList();

        }
        public string GetLoginUser(string LoginName, string Password)
        
        {
            // Initialization.  
            string status = null;
            try
            {

                var param = new SqlParameter[] {
                    new SqlParameter() {
                        ParameterName = "@pLoginName",
                        SqlDbType =  System.Data.SqlDbType.NVarChar,
                        Size = 100,
                        Direction = System.Data.ParameterDirection.Input,
                        Value = LoginName
                    },
                    new SqlParameter() {
                        ParameterName = "@pPassword",
                        SqlDbType =  System.Data.SqlDbType.NVarChar,
                        Direction = System.Data.ParameterDirection.Input,
                        Value = Password
                    }};
                // var context = new SchoolContext();
                using (var cnn = this.Database.GetDbConnection())
                {
                    var cmm = cnn.CreateCommand();
                    cmm.CommandType = System.Data.CommandType.StoredProcedure;
                    cmm.CommandText = "[dbo].[uspLogin]";
                    cmm.Parameters.AddRange(param);
                    cmm.Connection = cnn;
                    cnn.Open();
                    var reader = cmm.ExecuteReader();

                    while (reader.Read())
                    {
                        // name from student table 
                        status = Convert.ToString(reader["StatusName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return status;
        }

    }
}
