using Neshagostar.DAL.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neshagostar.WebUI.Areas.Storage.Controllers.MaterialsRelated
{
    public class MaterialsController : Controller
    {
        NeshagostarContext context = new NeshagostarContext();
        // GET: Storage/Materials

        public ActionResult Index()
        {
            var materials = context.MaterialPurchaseds.Include("MaterialDedicateds").Include("MaterialSeller").Include("MaterialSpecification").Include("MaterialImporteds").OrderByDescending(d => d.DateOfPurchase);
            return View(materials);
        }
    }
}