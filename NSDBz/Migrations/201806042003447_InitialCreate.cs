namespace NSDBz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImgUrl = c.String(),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Size = c.String(),
                        Hemisphere = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Species",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImgUrl = c.String(),
                        Name = c.String(nullable: false),
                        Class = c.String(),
                        Bio = c.String(),
                        Legs = c.Int(nullable: false),
                        Toxicity = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SpeciesCountry",
                c => new
                    {
                        Species_ID = c.Int(nullable: false),
                        Country_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Species_ID, t.Country_ID })
                .ForeignKey("dbo.Species", t => t.Species_ID, cascadeDelete: true)
                .ForeignKey("dbo.Country", t => t.Country_ID, cascadeDelete: true)
                .Index(t => t.Species_ID)
                .Index(t => t.Country_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpeciesCountry", "Country_ID", "dbo.Country");
            DropForeignKey("dbo.SpeciesCountry", "Species_ID", "dbo.Species");
            DropIndex("dbo.SpeciesCountry", new[] { "Country_ID" });
            DropIndex("dbo.SpeciesCountry", new[] { "Species_ID" });
            DropTable("dbo.SpeciesCountry");
            DropTable("dbo.Species");
            DropTable("dbo.Country");
        }
    }
}
