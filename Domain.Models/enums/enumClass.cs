using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.enums
{
    public enum Priority
    {
        None,
        Low,
        Medium,
        High,
        Urgent
    } 
    public enum Complexity
    {
        None,
        Minutes,
        Hours,
        Days,
        Weeks
    }
}
