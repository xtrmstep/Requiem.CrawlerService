namespace CrawlerService.Impl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityMessages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Message = c.String(),
                        Date = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CrawlRules",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        DataType = c.Int(nullable: false),
                        RegExpression = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DataBlocks",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Data = c.String(),
                        Date = c.DateTime(nullable: false),
                        Url_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UrlItems", t => t.Url_Id)
                .Index(t => t.Url_Id);
            
            CreateTable(
                "dbo.UrlItems",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Url = c.String(),
                        Host = c.String(),
                        IsInProgress = c.Boolean(nullable: false),
                        EvaliableFromDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DateStart = c.DateTime(nullable: false),
                        DateFinish = c.DateTime(),
                        Url_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UrlItems", t => t.Url_Id)
                .Index(t => t.Url_Id);
            
            CreateTable(
                "dbo.HostSettings",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Host = c.String(),
                        RobotsTxt = c.String(),
                        Disallow = c.String(),
                        CrawlDelay = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobItems", "Url_Id", "dbo.UrlItems");
            DropForeignKey("dbo.DataBlocks", "Url_Id", "dbo.UrlItems");
            DropIndex("dbo.JobItems", new[] { "Url_Id" });
            DropIndex("dbo.DataBlocks", new[] { "Url_Id" });
            DropTable("dbo.HostSettings");
            DropTable("dbo.JobItems");
            DropTable("dbo.UrlItems");
            DropTable("dbo.DataBlocks");
            DropTable("dbo.CrawlRules");
            DropTable("dbo.ActivityMessages");
        }
    }
}
