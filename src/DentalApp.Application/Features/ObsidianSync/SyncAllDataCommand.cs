using DentalApp.Application.Common.Interfaces;

namespace DentalApp.Application.Features.ObsidianSync;

public record SyncAllDataCommand(string VaultPath);
public class SyncAllDataHandler
{
    private readonly IMarkdownExporter _exporter;
    public SyncAllDataHandler(IMarkdownExporter exporter) => _exporter = exporter;
    public async Task Handle(SyncAllDataCommand command)
    {
        await _exporter.ExportAllToObsidianAsync(command.VaultPath);
    }
}