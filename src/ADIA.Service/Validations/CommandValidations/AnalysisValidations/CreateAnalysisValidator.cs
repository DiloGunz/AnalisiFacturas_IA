using ADIA.Model.DataTransfer.Commands.AnalysisCommands;
using FluentValidation;
using ADIA.Shared.Extensions;
using ADIA.Shared.Enums;

namespace ADIA.Service.Validations.CommandValidations.AnalysisValidations;

public class CreateAnalysisValidator : AbstractValidator<CreateAnalysisCommand>
{
    public CreateAnalysisValidator()
    {
        RuleFor(x => x.FileName).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Ingresar el nombre del archivo");
        RuleFor(x => x.File).NotNull().WithMessage("Ingresar un archivo válido.");
        RuleFor(x => x.FileExtension.GetFileType()).Must(x => x == EntityEnums.FileType.Pdf || x == EntityEnums.FileType.Image)
            .WithMessage("Ingresar un archivo del tipo JPG, PNG o PDG");
    }
}