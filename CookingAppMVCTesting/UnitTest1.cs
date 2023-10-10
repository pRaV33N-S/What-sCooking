using NUnit.Framework;
using CookingAppMVC.Controllers;
using CookingAppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Moq;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using System.Reflection;
using CookingAppAPI.Controllers;

namespace CookingAppMVCTests
{
    [TestFixture]
    public class AdminControllerTests
    {

        [SetUp]
        public void SetUp()
        {

        }
        [Test]
        public void Index_ReturnsAViewResult_WithAListOfAdmins()
        {
            // Arrange
            var controller = new AdminController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Admin>>(result.Model);
        }


        [Test]
        public void Details_ValidId_ReturnsViewResultWithAdminModel()
        {
            // Arrange
            var controller = new AdminController();
            var adminId = 1;

            // Act
            var result = controller.Details(adminId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Admin>(result.Model);
        }

    }




    /*   ===============================================================================================================
    */



    [TestFixture]
    public class LoginRegControllerTests
    {
        private LoginRegController _controller;
        private Mock<HttpContext> _httpContextMock;

        [SetUp]
        public void Setup()
        {
            /* _controller = new LoginRegController();*/
            _httpContextMock = new Mock<HttpContext>();

            var controllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };
            _controller = new LoginRegController { ControllerContext = controllerContext };
        }

        [Test]
        public void Login_Get_ReturnsViewResult()
        {
            var result = _controller.Login() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewName);
        }

        [Test]
        public void Login_Post_InvalidModel_ReturnsViewResult()
        {
            var invalidModel = new CookingAppMVC.Models.Login(); // Invalid model, missing required properties

            var result = _controller.Login(invalidModel) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewName);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
        }

        [Test]
        public void RegisterGetReturnsViewResult()
        {
            var result = _controller.Register() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewName);
        }

        [Test]
        public void Register_Get_ReturnsViewResult()
        {
            // Arrange 
            var invalidModel = new CookingAppMVC.Models.Register();

            // Act 
            var result = Task.Run(async () => await _controller.Register(invalidModel)).Result as ViewResult;

            // Assert 
            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewName);
        }



        [Test]
        public void ForgotPassword_Get_ReturnsViewResult()
        {
            var result = _controller.ForgotPassword() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewName);
        }


    }


    /*    ============================================================================================================================================
    */

    [TestFixture]
    public class RecipeControllerTests
    {
        private RecipeController _recipeController;

        [SetUp]
        public void Setup()
        {
            // Arrange
            _recipeController = new RecipeController();
        }

        [Test]
        public void Index_WithoutSearchString_ReturnsViewResult()
        {
            // Act
            var result = _recipeController.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Recipe>>(result.Model);
        }

        [Test]
        public void Index_WithSearchString_ReturnsFilteredViewResult()
        {
            // Act
            var result = _recipeController.Index("SearchString") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Recipe>>(result.Model);
        }

        [Test]
        public void Create_InvalidModel_ReturnsViewResultWithModel()
        {
            // Arrange
            _recipeController.ModelState.AddModelError("error", "some error");

            // Act
            var result = _recipeController.Create(new Recipe()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Recipe>(result.Model);
        }



        [TearDown]
        public void TearDown()
        {
            // Clean up resources after each test
            _recipeController.Dispose();
        }

        [Test]
        public void Feedback_Get_ReturnsViewResult()
        {
            // Act
            var result = _recipeController.Feedback() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Feedback>(result.Model);
        }


    }

}



