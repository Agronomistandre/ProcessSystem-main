using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProcessSystem.Contracts
{
    public class RegisterRequest: IBaseRequest
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public IList<string> ProcessTypesList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add(new ValidationResult("Имя витрины должно быть задано"));

            if (string.IsNullOrWhiteSpace(Url))
                errors.Add(new ValidationResult("Url для ответа пустой"));

            if (ProcessTypesList == null || ProcessTypesList.Count == 0)
                errors.Add(new ValidationResult("Список событий подписки пустой"));

            return errors;
        }
    }
}
