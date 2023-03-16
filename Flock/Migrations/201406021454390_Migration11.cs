namespace Flock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration11 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.User", "UserID", c => c.Int(nullable: false, identity: true));
            //AddColumn("dbo.User", "LanguageID", c => c.Byte(nullable: false));
            //AddColumn("dbo.User", "CountryID", c => c.Short(nullable: false));
            //AddColumn("dbo.User", "CreatedDT", c => c.DateTime(nullable: false));
            //AddColumn("dbo.User", "DeletedDT", c => c.DateTime());
            //AddColumn("dbo.User", "ColonyID", c => c.Long());
            //AddColumn("dbo.User", "Trade", c => c.Boolean(nullable: false));
            //AddColumn("dbo.User", "HardBounceDT", c => c.DateTime());
            //AddColumn("dbo.User", "UnsubscribedDT", c => c.DateTime());
            //AddColumn("dbo.User", "UserSourceID", c => c.Short(nullable: false));
            //AddColumn("dbo.User", "UserSyncDT", c => c.DateTime());
            //AddColumn("dbo.User", "Name", c => c.String());
            //AddColumn("dbo.User", "OldPassword", c => c.String());
            //AddColumn("dbo.User", "PasswordUpgraded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.User", "PasswordUpgraded");
            //DropColumn("dbo.User", "OldPassword");
            //DropColumn("dbo.User", "Name");
            //DropColumn("dbo.User", "UserSyncDT");
            //DropColumn("dbo.User", "UserSourceID");
            //DropColumn("dbo.User", "UnsubscribedDT");
            //DropColumn("dbo.User", "HardBounceDT");
            //DropColumn("dbo.User", "Trade");
            //DropColumn("dbo.User", "ColonyID");
            //DropColumn("dbo.User", "DeletedDT");
            //DropColumn("dbo.User", "CreatedDT");
            //DropColumn("dbo.User", "CountryID");
            //DropColumn("dbo.User", "LanguageID");
            //DropColumn("dbo.User", "UserID");
        }
    }
}
