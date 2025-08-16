using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.PagedResultsDTO
{
    public sealed class CursorPage<T>
    {
        public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
        public int? NextAfter { get; init; }
        public bool HasMore { get; init; }
    }
}
