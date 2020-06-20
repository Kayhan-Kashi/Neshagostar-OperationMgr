using Neshagostar.DAL.DataModel.CommerceRelated.MaterialRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.StorageRelated.MaterialRelated
{
    public class MaterialPurchased
    {
        public Guid Id { get; set; }
        public Guid MaterialSellerId { get; set; }
        public Guid MaterialSpecificationId { get; set; }

        [Display(Name = "کد")]
        public string Code { get; set; }

        [Display(Name = "عنوان")]
        public string Title
        {
            get
            {
                return  MaterialSeller.Name + " , " + AmountPurchased + " کیلوگرم , " + DateOfPurchase;
            }
        }

        [Display(Name = "عنوان")]
        public string TitleWithRemaining
        {
            get
            {
                return MaterialSeller.Name + ", " + MaterialSpecification.Specification +  " , مقدار کل : " + AmountPurchased + " کیلوگرم , مقدار مانده : " + RemainedAmount + " ,  " + DateOfPurchase;
            }
        }

        [Display(Name = "عنوان")]
        public string TitleWithNotImportedAmount
        {
            get
            {
                return MaterialSeller.Name + ", " + MaterialSpecification.Specification + " , مقدار کل : " + AmountPurchased + " کیلوگرم , مقدار مانده : " + ( AmountPurchased - AmountImported) + " ,  " + DateOfPurchase;
            }
        }

        [Display(Name = "تاریخ")]
        public string DateOfPurchase { get; set; }

        [Display(Name = "قیمت")]
        public double Price { get; set; }

        [Display(Name = "ارزش افزوده")]
        public double AddedCostPrice { get; set; }

        [Display(Name = "روش خرید")]
        public string PurchaseMethod { get; set; }

        [Display(Name = "کارمزد")]
        public double Escont { get; set; }

        [Display(Name = "هزینه های دیگر")]
        public double OtherCosts { get; set; }

        [Display(Name = "مقدار خریداری شده")]
        public double AmountPurchased { get; set; }

        [Display(Name = "مقدار مصرف شده")]
        public double AmountConsumed
        {
            get
            {
                if (MaterialDedicateds != null && MaterialDedicateds.Count > 0)
                {
                    double totalConsumed = 0;
                    foreach (var md in MaterialDedicateds)
                    {
                        totalConsumed += md.RealConsumedAmount;
                    }
                    return totalConsumed;
                }
                return 0;
            }
        }

        [Display(Name = "مقدار موجود")]
        public double AmountReady
        {
            get
            {
                double totalDedicatedsAmount = 0;
                double totalWithdrawedsAmount = 0;

                if (MaterialDedicateds != null && MaterialDedicateds.Count > 0)
                {            
                    foreach(var dedicated in MaterialDedicateds)
                    {
                        totalDedicatedsAmount += dedicated.AmountDedicated;
                        totalWithdrawedsAmount += dedicated.AmountWithdrawed;
                    }           
                }
                else
                {
                    return AmountPurchased;
                }
    
                return (AmountImported - totalDedicatedsAmount) + totalWithdrawedsAmount;
            }
        }

        [Display(Name = "مقدار دریافت شده")]
        public double AmountImported
        {
            get
            {
                double totalImportAmount = 0;
                if(MaterialImporteds != null && MaterialImporteds.Count > 0)
                {
                    foreach (var imported in MaterialImporteds)
                    {
                        totalImportAmount += imported.AmountImported + imported.LostAmount;
                    }
                }
                return totalImportAmount;
            }
        }

        [Display(Name = "مقدار دریافت شده واقعی")]
        public double RealAmountImported
        {
            get
            {
                double totalImportAmount = 0;
                if (MaterialImporteds != null && MaterialImporteds.Count > 0)
                {
                    foreach (var imported in MaterialImporteds)
                    {
                        totalImportAmount += imported.AmountImported;
                    }
                }
                return totalImportAmount;
            }
        }

        [Display(Name = "به طور کامل ارسال شده")]
        public bool IsImportedCompletely
        {
            get
            {
                if (AmountImported > 0)
                {
                    return AmountPurchased - AmountImported == 0;
                }

                return false;
        
            }
        }

        [Display(Name = "به طور کامل مصرف شده")]
        public bool IsConsumedCompletely
        {
            get
            {
                return AmountReady == 0;

            }
        }

        [Display(Name = "مقدار باقی مانده")]
        public double RemainedAmount
        {
            get
            {            
                if (RealAmountImported > 0)
                {
                    return RealAmountImported - AmountConsumed;
                }
                else
                {
                    return 0;
                }
            }
        }



        public MaterialSeller MaterialSeller { get; set; }
        public MaterialSpecification MaterialSpecification { get; set; }

        public List<MaterialDedicated> MaterialDedicateds { get; set; }
        public List<MaterialImported> MaterialImporteds { get; set; }
    }
}
