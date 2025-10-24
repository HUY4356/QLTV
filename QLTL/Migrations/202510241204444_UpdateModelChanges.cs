namespace QLTL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModelChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaoCaoVongQuaySaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DanhMucSachId = c.Int(nullable: false),
                        ThongKeLuotMuon = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhMucSaches", t => t.DanhMucSachId, cascadeDelete: true)
                .Index(t => t.DanhMucSachId);
            
            CreateTable(
                "dbo.DanhMucSaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NhomSachId = c.Int(nullable: false),
                        MaSach = c.String(),
                        TenSach = c.String(),
                        TacGia = c.String(),
                        DonGia = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NhomSaches", t => t.NhomSachId, cascadeDelete: true)
                .Index(t => t.NhomSachId);
            
            CreateTable(
                "dbo.NhomSaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaNhom = c.String(),
                        TenNhom = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BoSungTaiLieux",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeXuat = c.String(),
                        XuLy = c.String(),
                        GiaiPhap = c.String(),
                        NoiDung = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BaoCaoVongQuaySaches", "DanhMucSachId", "dbo.DanhMucSaches");
            DropForeignKey("dbo.DanhMucSaches", "NhomSachId", "dbo.NhomSaches");
            DropIndex("dbo.DanhMucSaches", new[] { "NhomSachId" });
            DropIndex("dbo.BaoCaoVongQuaySaches", new[] { "DanhMucSachId" });
            DropTable("dbo.BoSungTaiLieux");
            DropTable("dbo.NhomSaches");
            DropTable("dbo.DanhMucSaches");
            DropTable("dbo.BaoCaoVongQuaySaches");
        }
    }
}
