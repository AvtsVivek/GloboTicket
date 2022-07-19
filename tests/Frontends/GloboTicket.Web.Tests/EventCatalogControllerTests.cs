using GloboTicket.Web.Controllers;
using GloboTicket.Web.Models;
using GloboTicket.Web.Models.Api;
using GloboTicket.Web.Models.View;
using GloboTicket.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.Web.Tests
{
    public class EventCatalogControllerTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAnEventListModelObject()
        {
            // Arrange
            var categoryId = new Guid();
            var eventCatalogService = new Mock<IEventCatalogService>();
            var shoppingBasketService = new Mock<IShoppingBasketService>();
            var settings = new Settings();

            shoppingBasketService.Setup(basketService => basketService.GetBasket(settings.UserId)).ReturnsAsync(GetTestBasket());

            eventCatalogService.Setup(catalogService => catalogService.GetCategories()).ReturnsAsync(GetTestCategories());
            eventCatalogService.Setup(catalogService => catalogService.GetAll()).ReturnsAsync(GetTestEvents());
            eventCatalogService.Setup(catalogService => catalogService.GetByCategoryId(categoryId)).ReturnsAsync(GetTestEvents());

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Cookie", new CookieHeaderValue(settings.BasketIdCookieName, settings.UserId.ToString()).ToString());

            var controller = new EventCatalogController(eventCatalogService.Object,
                shoppingBasketService.Object, settings)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                }
            };

            

            // Act
            var result = await controller.Index(categoryId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EventListModel>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Categories.ToList().Count);
        }

        [Fact]
        public void SimpleTestTrial()
        {
            // Arrange
            Assert.Equal(2, 2);
        }

        private static Basket GetTestBasket()
        {
            var basket = new Basket
            {
                BasketId = Guid.NewGuid()
            };
            return basket;
        }

        private List<Category> GetTestCategories()
        {
            var categories = new List<Category>();
            categories.Add(new Category()
            {
                CategoryId = new Guid(),
                Name = "Music"
            });

            categories.Add(new Category()
            {
                CategoryId = new Guid(),
                Name = "Movees"
            });

            return categories;
        }

        private List<Event> GetTestEvents()
        {
            var events = new List<Event>();

            events.Add(new Event()
            {
                CategoryId = new Guid(),
                Name = "Music"
            });

            events.Add(new Event()
            {
                CategoryId = new Guid(),
                Name = "Movees"
            });

            return events;
        }

    }
}
