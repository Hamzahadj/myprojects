using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class auteur
    {
        public auteur()
        {
            this.media = new List<medium>();
        }

        public long Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public virtual ICollection<medium> media { get; set; }
    }
}
