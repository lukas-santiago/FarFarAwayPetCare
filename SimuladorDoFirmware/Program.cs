
string ApiKey = "MinhaApiKeyTop";
Guid UniqueDeviceId = Guid.Parse("fd8d872d-3045-47c8-8b24-9db2d4807b7c"); //Guid.NewGuid();
string shortUniqueDeviceId = GuidHelper.ToShortString(UniqueDeviceId); 


Console.WriteLine("Guid: " + UniqueDeviceId.ToString());
Console.WriteLine("Short Guid: " + shortUniqueDeviceId);
Console.WriteLine("ApiKey: " + ApiKey);

// Busca Configuração
// -> Se não encontra utiliza a antiga
// Seta a configuração
// Executa a Configuração