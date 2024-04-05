using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entitties;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendee
{
    public class RegisterAttendeeOnEventUseCase
    {
        //o certo seria ter uma classe de repositorio para acessar o dbcontext
        private readonly PassInDbContext _dbContext;
        public RegisterAttendeeOnEventUseCase()
        {
            _dbContext = new PassInDbContext();
        }
        public ResponseRegisterJson Execute(Guid eventId, RequestRegisterEventJson request)
        {
            Validate(eventId, request);

            var entity = new Attendee
            {
                Email = request.Email,
                Name = request.Name,
                Event_Id = eventId,
                //UtcNow cria um horario base
                Created_At = DateTime.UtcNow,
            };

            _dbContext.Attendees.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseRegisterJson
            {
                Id = entity.Id,
            };
        }

        private void Validate(Guid eventId, RequestRegisterEventJson request)
        {
            var eventEntity = _dbContext.Events.Find(eventId);
            if (eventEntity is null)
            {
                throw new NotFoundException("An event with this id doesn't exist");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ErrorOnValidationException("The name is invalid");
            }

            if(EmailIsValid(request.Email) == false)
            {
                throw new ErrorOnValidationException("The email is invalid");
            }

            var attendeeAlreadyRegistered = _dbContext
                .Attendees
                .Any(attendee => attendee.Email.Equals(request.Email) && attendee.Event_Id == eventId);

            if (attendeeAlreadyRegistered == true) 
            {
                throw new ConflictException("You cannot register twice in the same event");
            }

            var attendeesForEvent = _dbContext.Attendees.Count(attendee => attendee.Event_Id == eventId);
            if(attendeesForEvent == eventEntity.Maximum_Attendees)
            {
                throw new ConflictException("There is no more room for this event");
            }
        }

        private bool EmailIsValid(string email)
        {
            try
            {
                new MailAddress(email);

                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}
