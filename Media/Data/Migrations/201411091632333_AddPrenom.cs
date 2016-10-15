namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrenom : DbMigration
    {
        public override void Up()
        {
            AddColumn("auteur", "Prenom", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("auteur", "Prenom");
        }
    }
}
