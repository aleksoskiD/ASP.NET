using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TaskComment : BaseEntity
    {
        public string Comment { get; set; }

        public virtual int TaskId { get; set; }
        public virtual Task Task { get; set; }

        public int? UserId { get; set; }
    }
}
