using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class UserHistory
    {
        public Guid? Id { get; set; }
        public DateTime? LoginDate { get; set; }
        public string? Title { get; set; }
        public string? Browser { get; set; }
        public string? UserAction { get; set; }
    }
}
