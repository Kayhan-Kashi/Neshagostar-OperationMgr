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
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.FluentAPI
{
    public class EntityTypeConfigs
    {

    }

        #region CallManagement
        public class CallerMapping : EntityTypeConfiguration<Caller>
        {
            public CallerMapping()
            {
                this.HasKey(c => c.Id)
                    .HasMany(c => c.RecievedCalls)
                    .WithRequired(c => c.Caller)
                    .HasForeignKey(c => c.CallerId);
            }
        }

        public class RecievedCallMapping : EntityTypeConfiguration<RecievedCall>
        {
            public RecievedCallMapping()
            {
                this.HasKey(c => c.Id);

            }
        }

        public class DepartmentMapping : EntityTypeConfiguration<Department>
        {
            public DepartmentMapping()
            {
                this.HasKey(d => d.Id)
                    .HasMany(d => d.Personnels)
                    .WithRequired(p => p.Department)
                    .HasForeignKey(p => p.DepartmentId);
            }
        }
        #endregion

        #region CommerceRelated



    public class OrderMapping : EntityTypeConfiguration<Order>
    {
        public OrderMapping()
        {
            this.HasKey(c => c.Id)
                .HasMany(o => o.OrderItems)
                .WithRequired(o => o.Order)
                .HasForeignKey(o => o.OrderId);

            this.HasRequired(o => o.Customer)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.CustomerId);
        }
    }

    public class OrderItemMapping : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemMapping()
        {
            this.HasKey(c => c.Id)
                .HasRequired(o => o.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(o => o.OrderId).WillCascadeOnDelete(false);

            this.HasRequired(o => o.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(o => o.ProductId).WillCascadeOnDelete(false);

            this.HasMany(p => p.AllocatedProducts)
            .WithRequired(p => p.OrderItem)
            .HasForeignKey(p => p.OrderItemId).WillCascadeOnDelete(false);
        }
    }


    public class TenderItemMapping : EntityTypeConfiguration<TenderItem>
    {
        public TenderItemMapping()
        {
            this.HasKey(c => c.Id)
                .HasRequired(a => a.Tender)
                .WithMany(p => p.TenderItems)
                .HasForeignKey(a => a.TenderId);

            this.HasRequired(t => t.Product)
                .WithMany()
                .HasForeignKey(t => t.ProductId);

        }
    }

    public class TenderMapping : EntityTypeConfiguration<Tender>
    {
        public TenderMapping()
        {
            this.HasKey(c => c.Id)
                .HasMany(t => t.TenderItems)
                .WithRequired(t => t.Tender)
                .HasForeignKey(t => t.TenderId);

            this.HasRequired(t => t.Customer)
                .WithMany()
                .HasForeignKey(t => t.CustomerId);

            this.HasMany(t => t.RivalPrices)
                .WithRequired(r => r.Tender)
                .HasForeignKey(r => r.TenderId);

        }
    }

    public class RivalMapping : EntityTypeConfiguration<Rival>
    {
        public RivalMapping()
        {
            this.HasKey(o => o.Id)
                .HasMany(o => o.RivalPrices)
                .WithRequired(t => t.Rival)
                .HasForeignKey(o => o.RivalId);
        }
    }

    public class RivalPriceMapping : EntityTypeConfiguration<RivalPrice>
    {
        public RivalPriceMapping()
        {
            this.HasKey(o => o.Id)
            .HasRequired(t => t.Rival)
            .WithMany(t => t.RivalPrices)
            .HasForeignKey(t => t.RivalId);
        }
    }


    public class CustomerMapping : EntityTypeConfiguration<Customer>
    {
        public CustomerMapping()
        {
            this.HasKey(c => c.Id);
        }
    }


    public class ProductCategoryMapping : EntityTypeConfiguration<ProductCategory>
    {
        public ProductCategoryMapping()
        {
            this.HasKey(c => c.Id);
        }
    }

    public class RingStiffnessMapping : EntityTypeConfiguration<RingStiffness>
    {
        public RingStiffnessMapping()
        {
            this.HasKey(c => c.Id);
        }
    }

    public class CouplingMapping : EntityTypeConfiguration<Coupling>
    {
        public CouplingMapping()
        {
            this.HasKey(c => c.Id);
        }
    }

    public class PipeDiameterMapping : EntityTypeConfiguration<PipeDiameter>
    {
        public PipeDiameterMapping()
        {
            this.HasKey(c => c.Id);
        }
    }

    public class PipeProfileMapping : EntityTypeConfiguration<PipeProfile>
    {
        public PipeProfileMapping()
        {
            this.HasKey(c => c.Id);
        }
    }

    public class ProductMapping : EntityTypeConfiguration<Product>
    {
        public ProductMapping()
        {
            this.HasKey(c => c.Id)
                .HasRequired(p => p.ProductGenre)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.ProductGenreId)
                .WillCascadeOnDelete();

            //this.HasMany(p => p.DailyProductionReports)
            //     .WithRequired(p => p.Product)
            //     .WillCascadeOnDelete();



            this.HasRequired(p => p.RingStiffness)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.RingStiffnessId)
                .WillCascadeOnDelete();

            this.HasRequired(p => p.Coupling)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CouplingId)
                .WillCascadeOnDelete();


        }
    }

        public class ProductGenreMapping : EntityTypeConfiguration<ProductGenre>
        {
            public ProductGenreMapping()
            {
                this.HasKey(c => c.Id)
                    .HasRequired(p => p.PipeDiameter)
                    .WithMany(p => p.Products)
                    .HasForeignKey(p => p.PipeDiameterId)
                    .WillCascadeOnDelete();

                this.HasRequired(p => p.PipeProfile)
                  .WithMany(p => p.Products)
                  .HasForeignKey(p => p.PipeProfileId)
                  .WillCascadeOnDelete();

                this.HasRequired(p => p.ProductCategory)
               .WithMany(p => p.Products)
               .HasForeignKey(p => p.ProductCategoryId)
               .WillCascadeOnDelete();

            this.HasMany(p => p.Products)
                .WithRequired(p => p.ProductGenre)
                .WillCascadeOnDelete();



            }

        }



        public class InquiryItemMapping : EntityTypeConfiguration<InquiryItem>
        {
            public InquiryItemMapping()
            {
                this.HasKey(c => c.Id)
                    .HasRequired(i => i.Inquiry)
                    .WithMany(i => i.InquiryItems)
                    .HasForeignKey(i => i.InquiryId);

                this.HasRequired(i => i.Product)
                    .WithMany(p => p.InquiryItems)
                    .HasForeignKey(i => i.ProductId);
            }
        }

        public class InquiryMapping : EntityTypeConfiguration<Inquiry>
        {
            public InquiryMapping()
            {
                this.HasKey(c => c.Id)
                    .HasRequired(c => c.Customer)
                    .WithMany(c => c.Inquiries)
                    .HasForeignKey(i => i.CustomerId);
            }
        }

        public class MaterialSellerMapping : EntityTypeConfiguration<MaterialSeller>
        {
            public MaterialSellerMapping()
            {
                this.HasKey(c => c.Id)
                .HasMany(m => m.MaterialPurchaseds);                         
            }
        }

        public class MaterialSpecificationMapping : EntityTypeConfiguration<MaterialSpecification>
        {
            public MaterialSpecificationMapping()
            {
                this.HasKey(c => c.Id)
                .HasMany(m => m.MaterialPurchaseds);
            }
        }

        public class MaterialPurchasedMapping : EntityTypeConfiguration<MaterialPurchased>
        {
            public MaterialPurchasedMapping()
            {
                this.HasKey(c => c.Id)
                    .HasRequired(c => c.MaterialSeller)
                    .WithMany(c => c.MaterialPurchaseds)
                    .HasForeignKey(c => c.MaterialSellerId)
                    .WillCascadeOnDelete(false);

                this.HasRequired(c => c.MaterialSpecification)
                      .WithMany(c => c.MaterialPurchaseds)
                      .HasForeignKey(c => c.MaterialSpecificationId)
                      .WillCascadeOnDelete(false);
            }
        }


    #endregion

    #region ActivityLogging

    public class ActivityLogMapping : EntityTypeConfiguration<ActivityLog>
        {
            public ActivityLogMapping()
            {
                this.HasKey(c => c.Id)
                    .HasRequired(a => a.Personnel)
                    .WithMany(p => p.ActivityLogs)
                    .HasForeignKey(a => a.PersonnelId);
            }
        }

        public class JustifyMapping : EntityTypeConfiguration<Justify>
        {
            public JustifyMapping()
            {
                this.HasKey(c => c.Id)
                    .HasRequired(a => a.Personnel)
                    .WithMany(p => p.Justifies)
                    .HasForeignKey(a => a.PersonnelId);
            }
        }

    #endregion

        #region Storage


        public class SendingMapping : EntityTypeConfiguration<Sending>
    {
        public SendingMapping()
        {
            this.HasKey(s => s.Id)
                .HasRequired(s => s.OrderItem)
                .WithMany(o => o.Sendings)
                .HasForeignKey(s => s.OrderItemId)
                .WillCascadeOnDelete(false);

          
        }
    }

        public class ImportedProductMapping : EntityTypeConfiguration<ImportedProduct>
        {
            public ImportedProductMapping()
            {
                this.HasKey(c => c.Id)
                    .HasRequired(a => a.Product)
                    .WithMany()
                    .HasForeignKey(a => a.ProductId).WillCascadeOnDelete(false);

            this.HasMany(p => p.AllocatedProducts)
            .WithRequired(p => p.ImportedProduct)
            .HasForeignKey(p => p.ImportedProductId).WillCascadeOnDelete(false);
            }
        }

        public class AllocatedProductMapping : EntityTypeConfiguration<AllocatedProduct>
    {
        public AllocatedProductMapping()
        {
            this.HasKey(c => c.Id)
                .HasRequired(a => a.ImportedProduct)
                .WithMany()
                .HasForeignKey(a => a.ImportedProductId)
                .WillCascadeOnDelete(false);

            this.HasRequired(p => p.OrderItem)
            .WithMany()
            .HasForeignKey(p => p.OrderItemId)
             .WillCascadeOnDelete(false);
        }
    }

        public class SendingAllocatedMapping : EntityTypeConfiguration<SendingAllocated>
    {
        public SendingAllocatedMapping()
        {
            this.HasKey(c => c.Id)
                .HasRequired(s => s.AllocatedProduct)
                .WithMany(al => al.SendingAllocateds)
                .HasForeignKey(p => p.AllocatedProductId)
                .WillCascadeOnDelete(false);

            this.HasRequired(p => p.Sending)
                .WithMany(p => p.SendingAllocateds)
                .HasForeignKey(p => p.SendingId)
                .WillCascadeOnDelete(true);
        }
    }

        public class MaterialImportedMapping : EntityTypeConfiguration<MaterialImported>
        {
            public MaterialImportedMapping()
            {
                this.HasKey(c => c.Id)
                    .HasRequired(m => m.MaterialPurchased)
                    .WithMany(m => m.MaterialImporteds)
                    .WillCascadeOnDelete(false);

            }
        }

        public class MaterialDedicatedMapping : EntityTypeConfiguration<MaterialDedicated>
        {
            public MaterialDedicatedMapping()
            {
                this.HasKey(c => c.Id)
                    .HasRequired(m => m.MaterialPurchased)
                    .WithMany(m => m.MaterialDedicateds)
                    .WillCascadeOnDelete(false);
            }
        }

        public class ProductIssueMapping : EntityTypeConfiguration<ProductIssue>
        {
            public ProductIssueMapping()
            {
            this.HasKey(c => c.Id)
                .HasMany(p => p.ImportedUnStandardProducts)
                .WithOptional(p => p.ProductIssue)
            
                .HasForeignKey(p => p.ProductIssueId)
                .WillCascadeOnDelete(false);

                                      
            }
        }

    #region Wastage

    public class ImportedUnStandardProductMapping : EntityTypeConfiguration<ImportedUnStandardProduct>
    {
        public ImportedUnStandardProductMapping()
        {
            this.HasKey(c => c.Id)
                .HasOptional(i => i.DispatchedToBeGrindedProducts)
                .WithOptionalDependent()
                .WillCascadeOnDelete(false);

            this.HasOptional(i => i.ImportedProducts)
             .WithOptionalDependent()
             .WillCascadeOnDelete(false);

            this.HasOptional(i => i.OrderItem)
                .WithMany(i => i.ImportedUnStandardProducts)
                .HasForeignKey(i => i.OrderItemId)
                .WillCascadeOnDelete(false);

            this.HasRequired(i => i.Product)
                         .WithMany(i => i.ImportedUnStandardProducts)
                         .HasForeignKey(i => i.ProductId)
                         .WillCascadeOnDelete(false);

   
          
        }
    }

    public class DispatchedToBeGrindedProductMapping : EntityTypeConfiguration<DispatchedToBeGrindedProduct>
    {
        public DispatchedToBeGrindedProductMapping()
        {
            this.HasKey(c => c.Id);

        }
    }

    #endregion





    #endregion

    #region Financial
    public class PaymentMethodMapping : EntityTypeConfiguration<PaymentMethod>
        {
            public PaymentMethodMapping()
            {
                this.HasKey(c => c.Id);
            }
        }

        public class PaymentMapping : EntityTypeConfiguration<Payment>
        {
            public PaymentMapping()
            {
                this.HasKey(c => c.Id)
                    .HasRequired(c => c.Order)
                    .WithMany(c => c.Payments)
                    .HasForeignKey(c => c.OrderId);

                this.HasRequired(c => c.PaymentMethod)
                    .WithMany()
                    .HasForeignKey(c => c.PaymentMethodId);
            }
        }
        #endregion

        #region ProductionRelated
        public class SaloonMapping : EntityTypeConfiguration<Saloon>
        {
            public SaloonMapping()
            {
                this.HasKey(c => c.Id);
                
            }
        }

        public class DeviceMapping : EntityTypeConfiguration<Device>
        {
            public DeviceMapping()
            {
            this.HasKey(c => c.Id)
                .HasRequired(p => p.Saloon)
                .WithMany(p => p.Devices)
                .HasForeignKey(p => p.SaloonId).WillCascadeOnDelete();

            }
        }

        public class ProductionLineMapping : EntityTypeConfiguration<ProductionLine>
        {
            public ProductionLineMapping()
            {
            this.HasKey(c => c.Id)
                .HasRequired(p => p.Device)
                .WithMany(p => p.ProductionLines)
                .HasForeignKey(p => p.DeviceId).WillCascadeOnDelete();

                this.HasRequired(p => p.ProductGenre)
                    .WithMany()
                    .HasForeignKey(p => p.ProductGenreId)
                    .WillCascadeOnDelete(false);           
            }
        }

            public class DailyProductionReportMapping : EntityTypeConfiguration<DailyProductionReport>
        {
            public DailyProductionReportMapping()
            {
            this.HasKey(c => c.Id)
                .HasRequired(p => p.ProductionLine)
                .WithMany(p => p.DailyProductionReports)
                .HasForeignKey(p => p.ProductionLineId)
                .WillCascadeOnDelete(false);

            this.HasRequired(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId)
                .WillCascadeOnDelete(false);
        }
        }



        #endregion
    }

