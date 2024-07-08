using Core.CrossCuttingConcerns.Exceptions.Types;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = Core.CrossCuttingConcerns.Exceptions.Types.ValidationException;

namespace Core.Application.Pipelines.Validation;

public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators; //doğrulayıcılarımızın bir listesini tutuyoruz bir request'in birden fazla doğrulayıcısı olabileceği için IEnumerable olarak yazdık.

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ValidationContext<object> context = new(request);//doğrulanacak olan request

        IEnumerable<ValidationExceptionModel> errors = _validators
            .Select(validator => validator.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .GroupBy(
                f => f.PropertyName,
                f => f.ErrorMessage,
                (propertyName, errorMessages) => new ValidationExceptionModel
                {
                    Property = propertyName,
                    Errors = errorMessages
                }).ToList();

        if (errors.Any())
            throw new ValidationException(errors);

        TResponse response = await next(); // hata yoksa command'i çalıştır.hata yok sen çalışabilirsin.
        return response;
    }
}
