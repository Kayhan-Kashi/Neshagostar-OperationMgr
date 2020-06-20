using Microsoft.AspNet.Identity.EntityFramework;
using Neshagostar.DAL.DataModel.ActivityLogging.PersonnelActivity;
using Neshagostar.DAL.DataModel.ActivityTracker.PersonnelActivity;
using Neshagostar.DAL.DataModel.CallManagement;
using Neshagostar.DAL.DataModel.CommerceRelated.CustomersRelated;
using Neshagostar.DAL.DataModel.CommerceRelated.InquiriesRelated;
using Neshagostar.DAL.DataModel.CommerceRelated.MaterialRelated;
using Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated;
using Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated;
using Neshagostar.DAL.DataModel.CommerceRelated.TenderRelated;
using Neshagostar.DAL.DataModel.FinanceRelated;
using Neshagostar.DAL.DataModel.OrdersRelated.ProductsRelated;
using Neshagostar.DAL.DataModel.PersonnelRelated;
using Neshagostar.DAL.DataModel.ProductionRelated;
using Neshagostar.DAL.DataModel.StorageRelated.MaterialRelated;
using Neshagostar.DAL.DataModel.StorageRelated.OrdersRelated;
using Neshagostar.DAL.DataModel.StorageRelated.ProductsRelated;
using Neshagostar.DAL.DataModel.StorageRelated.WastageRelated;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel
{
    public class NeshagostarContext : IdentityDbContext
    {
        public NeshagostarContext() : base("NeshagostarDB")
        {


        }

        public static NeshagostarContext Create()
        {
            return new NeshagostarContext();
        }


        public DbSet<Personnel> IdentityUsers { get; set; }
        public DbSet<Caller> Callers { get; set; }
        public DbSet<RecievedCall> RecievedCalls { get; set; }
        public DbSet<Department> Departments { get; set; }


        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<RingStiffness> RingStiffnesses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PipeDiameter> PipeDiameters { get; set; }
        public DbSet<PipeProfile> PipeProfiles { get; set; }
        public DbSet<Coupling> Couplings { get; set; }
        public DbSet<ProductGenre> ProductGenres { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<InquiryItem> InquiryItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<Tender> Tenders { get; set; }
        public DbSet<TenderItem> TenderItems { get; set; }
        public DbSet<Rival> Rivals { get; set; }
        public DbSet<RivalPrice> RivalPrices { get; set; }

        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Justify> Justifies { get; set; }


        public DbSet<ImportedProduct> ImportedProducts { get; set; }
        public DbSet<AllocatedProduct> AllocatedProducts { get; set; }
        public DbSet<Sending> Sendings { get; set; }
        public DbSet<SendingAllocated> SendingAllocateds { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Saloon> Saloons { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<ProductionLine> ProductionLines { get; set; }
        public DbSet<DailyProductionReport> DailyProductionReports { get; set; }

        public DbSet<MaterialSeller> MaterialSellers { get; set; }
        public DbSet<MaterialSpecification> MaterialSpecifications { get; set; }
        public DbSet<MaterialImported> MaterialImporteds { get; set; }
        public DbSet<MaterialPurchased> MaterialPurchaseds { get; set; }
        public DbSet<MaterialDedicated> MaterialDedicateds { get; set; }

        public DbSet<ImportedUnStandardProduct> ImportedUnStandardProducts { get; set; }
        public DbSet<DispatchedToBeGrindedProduct> DispatchedToBeGrindedProducts { get; set; }
        public DbSet<ProductIssue> ProductIssues { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.CallerMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.RecievedCallMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.DepartmentMapping());

                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.CustomerMapping());

                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.ProductCategoryMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.RingStiffnessMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.PipeDiameterMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.PipeProfileMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.ProductMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.CouplingMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.ProductGenreMapping());


                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.InquiryItemMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.InquiryMapping());

                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.OrderItemMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.OrderMapping());

                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.RivalMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.RivalPriceMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.TenderItemMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.TenderMapping());

                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.ActivityLogMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.JustifyMapping());

                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.ImportedProductMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.AllocatedProductMapping());

                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.PaymentMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.PaymentMethodMapping());

                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.SaloonMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.DeviceMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.ProductionLineMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.DailyProductionReportMapping());

                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.MaterialSellerMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.MaterialSpecificationMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.MaterialDedicatedMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.MaterialPurchasedMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.MaterialImportedMapping());

                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.ImportedUnStandardProductMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.DispatchedToBeGrindedProductMapping());
                modelBuilder.Configurations.Add(new Neshagostar.DAL.FluentAPI.ProductIssueMapping());
            }
            catch
            {
                throw;
            }
            base.OnModelCreating(modelBuilder);
        }



        
    }
}
