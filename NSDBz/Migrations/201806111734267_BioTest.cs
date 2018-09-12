namespace NSDBz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BioTest : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Species", "Bio", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Species", "Bio", c => c.String());
        }
    }
}
