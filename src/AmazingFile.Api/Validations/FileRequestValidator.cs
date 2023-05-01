using AmazingFile.Api.Requests;
using FluentValidation;

namespace AmazingFile.Api.Validations;

public class FileRequestValidator : AbstractValidator<FileRequest>
{
    public FileRequestValidator()
    {
        RuleFor(o => o.InputPath).NotNull();
        RuleFor(o => o.OutputPath).NotNull();
    }
}
