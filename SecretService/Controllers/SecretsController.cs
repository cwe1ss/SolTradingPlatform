using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using SecretService.Models;

namespace SecretService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretsController : ControllerBase
    {
        /// <summary>
        /// Returns the secret for the given name.
        /// </summary>
        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(SecretResponse))]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Get(string name)
        {
            // The key vault url must be set either via launchSettings.json, via an app service config entry or via some other way that sets the environment variable.

            string kvUri = Environment.GetEnvironmentVariable("KEY_VAULT_URL") 
                           ?? throw new InvalidOperationException("Environment variable 'KEY_VAULT_URL' missing");

            // DefaultAzureCredential tries different methods to authenticate with the Azure Key Vault:
            // https://docs.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme#defaultazurecredential
            var azureCredential = new DefaultAzureCredential();

            var keyVaultSecretClient = new SecretClient(new Uri(kvUri), azureCredential);

            try
            {
                var secret = await keyVaultSecretClient.GetSecretAsync(name);
                
                return Ok(new SecretResponse
                {
                    SecretValue = secret.Value.Value,
                });
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == StatusCodes.Status404NotFound)
            {
                return NotFound();
            }
        }
    }
}
