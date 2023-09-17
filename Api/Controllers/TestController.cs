using Api.Biz;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
public class TestController: Controller
{
    private FirebaseService _firebaseService;

    public TestController(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
    }

    [HttpGet]
    [Route("LoadStudents")]
    public async Task LoadStudents()
    {
        await _firebaseService.Save("a",new Student(){Number = "12124129710"});
    }
}