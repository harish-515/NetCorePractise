using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ToDo.Data
{
    public class TodoItem : BaseEntity
    {
        public string Description { get; set; }

        [DefaultValue((int)PriorityType.Low)]
        public PriorityType Priority { get; set; }
        
        [DefaultValue((int)StatusType.Draft)]
        public StatusType Status { get; set; }
        
        public string PlannedDate { get; set; }
        
        public string CompletedDate { get; set; }
    }
}
