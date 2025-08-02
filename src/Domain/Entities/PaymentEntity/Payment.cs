using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PaymentEntity
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public float Amount { get; set; }

        public string TransactionId { get; set; }

        public bool Success { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
