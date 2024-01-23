using Azure.Security.KeyVault.Secrets;

public interface IAzureKeyVaultService
{
    Task<string> GetSecretAsync(string secretName);
}

public class AzureKeyVaultService : IAzureKeyVaultService
{
    private readonly SecretClient _secretClient;

    public AzureKeyVaultService(SecretClient secretClient)
    {
        _secretClient = secretClient;
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
        return secret.Value;
    }
}
