using System.Text;
using Api.Biz;
using Api.Controllers.Models;
using Api.Crawler.Ted;
using Api.Services;
using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Models
{
}

namespace Api.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private EncryptionService _encryptionService;
        private TedClient _tedClient;
        private FirebaseService _firebaseService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(EncryptionService encryptionService, TedClient tedClient, ILogger<AuthController> logger, FirebaseService firebaseService)
        {
            _encryptionService = encryptionService;
            _tedClient = tedClient;
            _logger = logger;
            _firebaseService = firebaseService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<CreateTokenResult> Login([FromBody] LoginRequest request)
        {
            bool success;
            string token = "";
        
            string firebaseToken = request.Token;
        
            _logger.LogInformation("Token:\n"+firebaseToken);
            
            var firebaseAuth = FirebaseAuth.DefaultInstance;
            FirebaseToken decodedToken = await firebaseAuth
                .VerifyIdTokenAsync(firebaseToken);
            string firebaseUserId = decodedToken.Uid;
        
            // todo SOLID
            try
            {
                var student = await _tedClient.LoadStudent(request.UserName, request.Password);
                await _firebaseService.Save(firebaseUserId,student);
                success = true;
                token = Encrypt(request.UserName, request.Password);
            }
            catch (Exception e)
            {
                // todo add message by exception 
                Console.WriteLine(e);
                success = false;
            }
        
            var createTokenResult = new CreateTokenResult() { Token = token, Success = success };

            return createTokenResult;
        }

        // Get username and password and generate encrypted string
        private string Encrypt(string username, string password)
        {
            var encryptCredentials = _encryptionService.EncryptCredentials(username, password);
            // encode using base64
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptCredentials));
        }

        private Credential Decrypt(string encrypted)
        {
            // decode encrypted parameter using base64
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encrypted));
            var decryptCredentials = _encryptionService.DecryptCredentials(decoded);
            return decryptCredentials;
        }
    }
}