namespace NSDBz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NSDBz3Test : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Species", "ImgUrl", c => c.String(nullable: false));
            AlterColumn("dbo.Species", "Class", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Species", "Class", c => c.String());
            AlterColumn("dbo.Species", "ImgUrl", c => c.String());
        }
    }
}
