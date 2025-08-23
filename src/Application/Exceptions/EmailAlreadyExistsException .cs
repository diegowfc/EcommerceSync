using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public sealed class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException(string email)
            : base("Já existe um usuário cadastrado com esse e-mail.") => Email = email;

        public string Email { get; }
    }
}
