namespace Neshagostar.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ImportedUnStandardProducts", "IssueSolved");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ImportedUnStandardProducts", "IssueSolved", c => c.String());
        }
    }
}
