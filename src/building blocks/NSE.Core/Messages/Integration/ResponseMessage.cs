using FluentValidation.Results;

namespace NSE.Core.Messages.Integration
{
    public class ResponseMessage : Message
    {
        public ValidationResult ValidationResult;

        public ResponseMessage(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }
    }
}
