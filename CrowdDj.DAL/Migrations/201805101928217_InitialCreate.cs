namespace CrowdDj.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 200),
                        Password = c.String(nullable: false, maxLength: 200),
                        EmailAddress = c.String(nullable: false, maxLength: 200),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Guests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(nullable: false, maxLength: 200),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Party_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parties", t => t.Party_Id)
                .Index(t => t.Party_Id);
            
            CreateTable(
                "dbo.Parties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Location = c.String(nullable: false, maxLength: 200),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlayLists",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parties", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Tracks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Interpret = c.String(nullable: false, maxLength: 200),
                        Url = c.String(nullable: false, maxLength: 200),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        RecommendedByGuest_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartyGuests", t => t.RecommendedByGuest_Id)
                .Index(t => t.RecommendedByGuest_Id);
            
            CreateTable(
                "dbo.PartyGuests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartyCode = c.Int(nullable: false),
                        GuestId = c.Int(nullable: false),
                        PartyId = c.Int(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Guests", t => t.GuestId, cascadeDelete: true)
                .ForeignKey("dbo.Parties", t => t.PartyId, cascadeDelete: true)
                .Index(t => t.GuestId)
                .Index(t => t.PartyId);
            
            CreateTable(
                "dbo.PartyTweets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 300),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        PartyGuest_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartyGuests", t => t.PartyGuest_Id, cascadeDelete: true)
                .Index(t => t.PartyGuest_Id);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        GuestId = c.Int(nullable: false),
                        TrackId = c.Int(nullable: false),
                        PlayListId = c.Int(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Guests", t => t.GuestId, cascadeDelete: true)
                .ForeignKey("dbo.PlayLists", t => t.PlayListId, cascadeDelete: true)
                .ForeignKey("dbo.Tracks", t => t.TrackId, cascadeDelete: true)
                .Index(t => t.GuestId)
                .Index(t => t.TrackId)
                .Index(t => t.PlayListId);
            
            CreateTable(
                "dbo.TrackPlayLists",
                c => new
                    {
                        Track_Id = c.Int(nullable: false),
                        PlayList_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Track_Id, t.PlayList_Id })
                .ForeignKey("dbo.Tracks", t => t.Track_Id, cascadeDelete: true)
                .ForeignKey("dbo.PlayLists", t => t.PlayList_Id, cascadeDelete: true)
                .Index(t => t.Track_Id)
                .Index(t => t.PlayList_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "TrackId", "dbo.Tracks");
            DropForeignKey("dbo.Votes", "PlayListId", "dbo.PlayLists");
            DropForeignKey("dbo.Votes", "GuestId", "dbo.Guests");
            DropForeignKey("dbo.Tracks", "RecommendedByGuest_Id", "dbo.PartyGuests");
            DropForeignKey("dbo.PartyTweets", "PartyGuest_Id", "dbo.PartyGuests");
            DropForeignKey("dbo.PartyGuests", "PartyId", "dbo.Parties");
            DropForeignKey("dbo.PartyGuests", "GuestId", "dbo.Guests");
            DropForeignKey("dbo.TrackPlayLists", "PlayList_Id", "dbo.PlayLists");
            DropForeignKey("dbo.TrackPlayLists", "Track_Id", "dbo.Tracks");
            DropForeignKey("dbo.PlayLists", "Id", "dbo.Parties");
            DropForeignKey("dbo.Guests", "Party_Id", "dbo.Parties");
            DropIndex("dbo.TrackPlayLists", new[] { "PlayList_Id" });
            DropIndex("dbo.TrackPlayLists", new[] { "Track_Id" });
            DropIndex("dbo.Votes", new[] { "PlayListId" });
            DropIndex("dbo.Votes", new[] { "TrackId" });
            DropIndex("dbo.Votes", new[] { "GuestId" });
            DropIndex("dbo.PartyTweets", new[] { "PartyGuest_Id" });
            DropIndex("dbo.PartyGuests", new[] { "PartyId" });
            DropIndex("dbo.PartyGuests", new[] { "GuestId" });
            DropIndex("dbo.Tracks", new[] { "RecommendedByGuest_Id" });
            DropIndex("dbo.PlayLists", new[] { "Id" });
            DropIndex("dbo.Guests", new[] { "Party_Id" });
            DropTable("dbo.TrackPlayLists");
            DropTable("dbo.Votes");
            DropTable("dbo.PartyTweets");
            DropTable("dbo.PartyGuests");
            DropTable("dbo.Tracks");
            DropTable("dbo.PlayLists");
            DropTable("dbo.Parties");
            DropTable("dbo.Guests");
            DropTable("dbo.Administrators");
        }
    }
}
