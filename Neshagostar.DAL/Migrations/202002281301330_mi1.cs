namespace Neshagostar.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mi1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductIssues",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ImportedUnStandardProducts", "ProductIssueId", c => c.Guid());
            CreateIndex("dbo.ImportedUnStandardProducts", "ProductIssueId");
            AddForeignKey("dbo.ImportedUnStandardProducts", "ProductIssueId", "dbo.ProductIssues", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImportedUnStandardProducts", "ProductIssueId", "dbo.ProductIssues");
            DropIndex("dbo.ImportedUnStandardProducts", new[] { "ProductIssueId" });
            DropColumn("dbo.ImportedUnStandardProducts", "ProductIssueId");
            DropTable("dbo.ProductIssues");
        }
    }
}
