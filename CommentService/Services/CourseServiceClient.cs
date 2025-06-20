using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace CommentService.Services
{
    public class CourseServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseServiceClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CourseExistsAsync(int courseId)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(token))
            {
                // Remove o "Bearer " antes de setar o valor
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            }

            var response = await _httpClient.GetAsync($"http://localhost:5072/api/Course/{courseId}");

            return response.IsSuccessStatusCode;
        }
    }
}