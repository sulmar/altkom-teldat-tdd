using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }

    public class Vehicle : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public bool IsRemoved { get; set; }
    }
}
