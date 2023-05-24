using System;
namespace Catalog.UnitTests;
using Catalog.Repositories;
using Catalog.Entities;
using Moq;
using Microsoft.Extensions.Logging;
using Catalog.Controllers;
using Microsoft.AspNetCore.Mvc;
using Catalog.Dtos;
using FluentAssertions;

public class ItemsControllerTest
{

    private readonly Mock<IItemsRepository> repositoryStub = new();
    private readonly Mock<ILogger<ItemsController>> loggerStub = new();
    private readonly Random rand = new();

    [Fact]
    public async Task GetItemsAsync_WithUnexistingItem_ReturnsNotFound()
    {
        //Arrange
        repositoryStub.Setup(repo => repo.GetItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Items?)null);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        //Act
        var result = await controller.GetItemsAsync(Guid.NewGuid());

        //Assert
        result.Result.Should().BeOfType<NotFoundResult>();

        // Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetItemsAsync_WithExistingItem_ReturnsExpectedItem()
    {
        //Arrange
        var expectedItem = CreateRandomItem();

        repositoryStub.Setup(repo => repo.GetItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(expectedItem);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        //Act
        var result = await controller.GetItemsAsync(Guid.NewGuid());

        //Assert
        result.Value.Should().BeEquivalentTo(expectedItem);

        // Assert.IsType<ItemDto>(result.Value);
        // var dto = (result as ActionResult<ItemDto>).Value;
        // Assert.Equal(expectedItem.Id, dto.Id);
        // Assert.Equal(expectedItem.Name, dto.Name);
        // Assert.Equal(expectedItem.Price, dto.Price);
        // Assert.Equal(expectedItem.CreatedDate, dto.CreatedDate);

    }

    [Fact]
    public async Task GetItemsAsync_WithExistingItem_ReturnsAllItems()
    {
        //Arrange
        var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };

        repositoryStub.Setup(repo => repo.GetItemsAsync()).ReturnsAsync(expectedItems);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        //Act
        var actualItems = await controller.GetItemsAsync();

        //Assert
        actualItems.Should().BeEquivalentTo(expectedItems);

    }

    [Fact]
    public async Task GetItemsAsync_WithMatchingItem_ReturnsMatchingItems()
    {
        //Arrange
        var allItems = new[] {
            new Items() {Name="Boots"},
            new Items() {Name="Boots of Travel"},
            new Items() {Name="Null Talisman"}
        };

        var nameToMatch = "Boots";

        repositoryStub.Setup(repo => repo.GetItemsAsync()).ReturnsAsync(allItems);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        //Act
        IEnumerable<ItemDto> foundItems = await controller.GetItemsAsync(nameToMatch);

        //Assert
        foundItems.Should().OnlyContain(
            item => item.Name == allItems[0].Name || item.Name == allItems[1].Name
        );

    }

    [Fact]
    public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
    {
        //Arrange
        var ItemToCreate = new CreateItemDto(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            rand.Next(9000));

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        //Act
        var result = await controller.CreateItemAsync(ItemToCreate);

        //Assert
        var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;

        ItemToCreate.Should().BeEquivalentTo(
            createdItem,
            options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
        );
        createdItem.Id.Should().NotBeEmpty();
        createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(1000));
    }

    [Fact]
    public async Task UpdateItemAsync_WithItemToCreate_ReturnsNoContent()
    {
        //Arrange
        var existingItem = CreateRandomItem();

        repositoryStub.Setup(repo => repo.GetItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(existingItem);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        var itemId = existingItem.Id;

        var itemToUpdate = new UpdateItemDto(
            Guid.NewGuid().ToString(),
            existingItem.Description,
            existingItem.Price + 10);

        //Act
        var result = await controller.UpdateItemAsync(itemId, itemToUpdate);

        //Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteItemAsync_WithItemToCreate_ReturnsNoContent()
    {
        //Arrange
        var existingItem = CreateRandomItem();

        repositoryStub.Setup(repo => repo.GetItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(existingItem);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        //Act
        var result = await controller.DeleteItemAsync(existingItem.Id);

        //Assert
        result.Should().BeOfType<NoContentResult>();
    }

    private Items CreateRandomItem()
    {

        return new()
        {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            Price = rand.Next(9000),
            CreatedDate = DateTimeOffset.UtcNow,
        };

    }
}