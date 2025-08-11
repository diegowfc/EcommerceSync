using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Event
{
    public record UserRegisterCommand
    {
        public Guid CorrelationId { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string PasswordHash { get; init; }
    }
}
