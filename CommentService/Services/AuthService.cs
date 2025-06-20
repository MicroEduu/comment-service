using System.Net.Http;
using System.Net.Http.Json;

namespace CommentService.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync(string email, string password)
        {
            var loginData = new { email, password };

            var response = await _httpClient.PostAsJsonAsync("http://localhost:5089/api/Auth/login", loginData);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao autenticar: {response.StatusCode} - {errorContent}");
            }

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

            return loginResponse?.Token ?? throw new Exception("Token não recebido na resposta");
        }
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; }
    }
}
