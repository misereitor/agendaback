using agendaback.Model.Request;
using FluentValidation;

namespace agendaback.Validations
{
    public class PasswordValidator : AbstractValidator<PasswordModel>
    {
        public PasswordValidator()
        {

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.")
                .MaximumLength(20).WithMessage("A senha deve ter no máximo 20 caracteres.")
                .Must(SpecialCharacters).WithMessage("A senha deve conter pelo menos um caractere especial.")
                .Must(SmallLetters).WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
                .Must(CapitalLetters).WithMessage("A senha deve conter pelo menos uma letra minúscula.")
                .Must(NumericDigits).WithMessage("A senha deve conter pelo menos um dígito.");
        }
        private bool SpecialCharacters(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Any(c => !char.IsLetterOrDigit(c));
        }

        private bool SmallLetters(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Any(c => char.IsUpper(c));
        }

        private bool CapitalLetters(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Any(c => char.IsLower(c));
        }

        private bool NumericDigits(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Any(c => char.IsDigit(c));
        }
    }
}
