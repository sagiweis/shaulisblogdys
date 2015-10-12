namespace ShaulisBlogDYS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                        AuthorSiteUrl = c.String(),
                        CommentText = c.String(),
                        PostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .Index(t => t.PostID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Author = c.String(),
                        Title = c.String(),
                        SiteUrl = c.String(),
                        PostDate = c.DateTime(nullable: false),
                        Content = c.String(),
                        Image = c.Binary(),
                        Video = c.Binary(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Fans",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.Boolean(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        TimeInClub = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "PostID", "dbo.Posts");
            DropIndex("dbo.Comments", new[] { "PostID" });
            DropTable("dbo.Fans");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
        }
    }
}
