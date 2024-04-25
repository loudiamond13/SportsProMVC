using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SportsPro.Controllers;
using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;
using System.Linq.Expressions;

namespace SportsPro.Tests.ControllerTests
{
    //test using xUnit
    
    public class ProductControllerTests
    {
        private IUnitOfWork _fakeUnitOfWork;
        private ProductController _productController;
        public ProductControllerTests()
        {
            //dependency
            _fakeUnitOfWork = A.Fake<IUnitOfWork>();

            //SUT / system under test
            _productController = new ProductController(_fakeUnitOfWork);
        }

        [Fact]
        public async Task ProductController_List_ReturnsSuccess() 
        {
            //arrange
            var products = A.Fake<List<Product>>();
            A.CallTo(() => _fakeUnitOfWork.Products.GetAll(null)).Returns(products);
            //act
            var result = await _productController.List("",null);

            //assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void ProductController_Edit_ReturnsViewResult_WithValidProduct()
        {
            //arrange
            int productID = 1;
            var product = new Product
            {
                ProductID = productID,
                ProductCode = "TEST",
                Name = "Name",
                YearlyPrice = 123,
                ReleaseDate = DateTime.Now,
            };
            A.CallTo(()=> _fakeUnitOfWork.Products.Find(A<Expression<Func<Product, bool>>>.Ignored,null)).Returns(product);

            //act
            var result = _productController.Edit(productID);


            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("AddEdit", viewResult.ViewName); //assert that the view name is "AddEdit"

            // Assert model passed to the view
            var model = Assert.IsAssignableFrom<Product>(viewResult.Model); //assert that the model passed to the view is of type Product
            Assert.Equal(productID, model.ProductID);//assert that the model has the correct ProductID
        }

        [Fact]
        public void ProductController_Adds_New_Product()
        {
            //arrange
            var newProduct = new Product 
            { 
                ProductID = 0, 
                ProductCode = "Test", 
                Name = "Test", 
                YearlyPrice = 123, 
                ReleaseDate = DateTime.Now,
            };

            //act
            var result = _productController.AddEdit(newProduct);

            //assert
            A.CallTo(() => _fakeUnitOfWork.Products.Add(newProduct)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _fakeUnitOfWork.Save()).MustHaveHappenedOnceExactly(); 
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().Be("List"); //make sure it goes back to list action
            
        }

        [Fact]
        public void ProductContoller_Edits_Existing_Product()
        {
            //arrange- create an existing product
            var existingProduct = new Product
            {
                ProductID = 1,
                ProductCode = "Test",
                Name = "Test",
                YearlyPrice = 123,
                ReleaseDate = DateTime.Now,
            };

            A.CallTo(() => _fakeUnitOfWork.Products.Find(A<Expression<Func<Product, bool>>>.Ignored, null)).Returns(existingProduct);

            //act
            var result = _productController.AddEdit(existingProduct);

            //assert
            A.CallTo(() => _fakeUnitOfWork.Save()).MustHaveHappenedOnceExactly();
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().Be("List", "Product");//should go to list action
        }

        [Fact]
        public void ProductController_Delete_An_Existing_Product() 
        {
            //arrange
            var existingProduct = new Product
            {
                ProductID = 1,
                ProductCode = "Test",
                Name = "Test",
                YearlyPrice = 123,
                ReleaseDate = DateTime.Now,
            };

            A.CallTo(() => _fakeUnitOfWork.Products.Find(A<Expression<Func<Product, bool>>>.Ignored, null)).Returns(existingProduct);

            //act
            var result = _productController.Delete(existingProduct.ProductID);

            //assert
            A.CallTo(() => _fakeUnitOfWork.Products.Delete(existingProduct)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _fakeUnitOfWork.Save()).MustHaveHappenedOnceExactly();
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().Be("List", "Product");//should go to list action
        }

    }
}
