using AutoMapper;
using Domain.Entities.UserEntity;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;

namespace Application.Consumers
{
    public class RegisterUserConsumer(IUnitOfWork _unitOfWork, IMapper mapper) : IConsumer<UserRegisterCommand>
    {
        public async Task Consume(ConsumeContext<UserRegisterCommand> context)
        {
            var evt = context.Message;

            var user = new User
            {
                Name = evt.Name,
                Email = evt.Email,
                PasswordHash = evt.PasswordHash                
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();
 
        }
    }
}
