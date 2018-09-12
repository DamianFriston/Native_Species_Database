namespace NSDBz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllReq : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Country", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Country", "Description", c => c.String());
        }
    }
}
