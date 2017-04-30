namespace CrawlerService.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataBlocks",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Url = c.String(),
                        Type = c.String(),
                        Data = c.String(),
                        ExtractedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DomainNames",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        EvaliableFromDate = c.DateTime(),
                        Allow = c.String(),
                        Disallow = c.String(),
                        CrawlDelay = c.Single(),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.ExtractRules",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Description = c.String(),
                        Type = c.String(),
                        RegExpression = c.String(),
                        Domain_Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DomainNames", t => t.Domain_Name)
                .Index(t => t.Domain_Name);
            
            CreateTable(
                "dbo.Processes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Status = c.String(),
                        DateStart = c.DateTime(nullable: false),
                        DateFinish = c.DateTime(),
                        Domain_Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DomainNames", t => t.Domain_Name)
                .Index(t => t.Domain_Name);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Processes", "Domain_Name", "dbo.DomainNames");
            DropForeignKey("dbo.ExtractRules", "Domain_Name", "dbo.DomainNames");
            DropIndex("dbo.Processes", new[] { "Domain_Name" });
            DropIndex("dbo.ExtractRules", new[] { "Domain_Name" });
            DropTable("dbo.Processes");
            DropTable("dbo.ExtractRules");
            DropTable("dbo.DomainNames");
            DropTable("dbo.DataBlocks");
        }
    }
}
