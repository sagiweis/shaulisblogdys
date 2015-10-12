namespace ShaulisBlogDYS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VideoUrl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "Video", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "Video", c => c.Binary());
        }
    }
}
