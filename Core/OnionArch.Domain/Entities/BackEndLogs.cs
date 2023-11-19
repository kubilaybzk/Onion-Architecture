using OnionArch.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class BackEndLogs:BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new Guid ID
        {
            get { return base.ID; }
            set { base.ID = value; }
        }

        public string? Message { get; set; }
        public string? MessageTemplate { get; set; }
        public string? Level { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string? Exception { get; set; }
        public string? Properties { get; set; }
        public string? EmailOrUserNameLogs { get; set; }

        [NotMapped]
        public new DateTime? CreateTime { get; set; }

        [NotMapped]
        public new DateTime? UpdateTime { get; set; }
    }
}
