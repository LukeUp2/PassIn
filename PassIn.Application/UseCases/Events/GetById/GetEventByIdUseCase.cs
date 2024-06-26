﻿using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.GetById
{
    public class GetEventByIdUseCase
    {
        public ResponseEventJson Execute(Guid id)
        {
            var dbContext = new PassInDbContext();

            var entity = dbContext.Events.Find(id);
            //alt -> dbContext.Events.FirstOrDefault(e => e.Id == id);

            //As messagens de erro deveriam ser Localizadas (idioma)
            if (entity is null)
                throw new NotFoundException("An event with this id doesn't exist");

            return new ResponseEventJson
            {
                Id = entity.Id,
                Title = entity.Title,
                Details = entity.Details,
                MaximumAttendees = entity.Maximum_Attendees,
                AttendeesAmount = -1,
            };
        }
    }
}
