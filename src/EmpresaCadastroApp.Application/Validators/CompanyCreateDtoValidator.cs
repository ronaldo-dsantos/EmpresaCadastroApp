using EmpresaCadastroApp.Application.DTOs.Company;
using FluentValidation;

namespace EmpresaCadastroApp.Application.Validators
{
    public class CompanyCreateDtoValidator : AbstractValidator<CompanyCreateDto>
    {
        public CompanyCreateDtoValidator()
        {
            RuleFor(x => x.Cnpj)
                .NotEmpty().WithMessage("O CNPJ é obrigatório.")
                .Must(ValidarCnpj).WithMessage("O CNPJ informado não é válido.");
        }

        private bool ValidarCnpj(string cnpj)
        {
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            if (cnpj.Length != 14)
                return false;

            var multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCnpj = cnpj.Substring(0, 12);
            var soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            var resto = (soma % 11);
            var digito1 = (resto < 2) ? 0 : 11 - resto;

            tempCnpj += digito1;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            var digito2 = (resto < 2) ? 0 : 11 - resto;

            return cnpj.EndsWith($"{digito1}{digito2}");
        }
    }
}
