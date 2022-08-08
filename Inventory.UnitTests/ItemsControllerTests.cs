using FluentAssertions;
using Inventory.Api.Controllers;
using Inventory.Api.Dtos;
using Inventory.Api.Entities;
using Inventory.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Inventory.UnitTests;
public class ItemsControllerTests
{

    private readonly Mock<IItemsRepository> repositoryStub = new();
    private readonly Mock<ILogger<ItemsController>> loggerStub = new();
    private readonly Random rand = new();

    [Fact]
    // UnitOfWork_StateUnderTest_ExpectedBehavior
    public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
    {
        // Arrange
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Item)null);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.GetItemAsync(Guid.NewGuid());

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    // UnitOfWork_StateUnderTest_ExpectedBehavior
    public async Task GetItemAsync_WithexistingItem_ReturnsExpectedItem()
    {
        // Arrange
        var expectedItem = CreateRandomItem();
        
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync(expectedItem);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.GetItemAsync(Guid.NewGuid());

        // Assert
        // expectedItem is record type so add options
        result.Value.Should().BeEquivalentTo(expectedItem);
    }

    [Fact]
    // UnitOfWork_StateUnderTest_ExpectedBehavior
    public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
    {
        // Arrange
        var expectedItems = new[]{CreateRandomItem(), CreateRandomItem(), CreateRandomItem()};
        
        repositoryStub.Setup(repo => repo.GetItemsAsync())
            .ReturnsAsync(expectedItems);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var actualItems = await controller.GetItemsAsync();

        // Assert
        actualItems.Should().BeEquivalentTo(expectedItems);
    }

      [Fact]
    // UnitOfWork_StateUnderTest_ExpectedBehavior
    public async Task GetItemsAsync_WithMatchingItems_ReturnsMatchingItems()
    {
        // Arrange
        var allItems = new[]
        {
            new Item(){Name = "Black Shirt"},
            new Item(){Name = "Blue Shirt"},
            new Item(){Name = "Black Hat"}
        };
        
        var nameToMatch = "Shirt";

        repositoryStub.Setup(repo => repo.GetItemsAsync())
            .ReturnsAsync(allItems);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        IEnumerable<ItemDto> foundItems = await controller.GetItemsAsync(nameToMatch);

        // Assert
        foundItems.Should().OnlyContain(item => item.Name == allItems[0].Name);
    }

    [Fact]
    // UnitOfWork_StateUnderTest_ExpectedBehavior
    public async Task CreatedItemAsync_WithItemToCreate_ReturnsAllItems()
    {
        // Arrange
        var itemToCreate = new CreateItemDto(Guid.NewGuid().ToString(),Guid.NewGuid().ToString(),rand.Next(10000));

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.CreatedItemAsync(itemToCreate);

        // Assert
        // expectedItem is record type so add options
        var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
        itemToCreate.Should().BeEquivalentTo(
            createdItem,
            options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
            );
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(1000));
    }

    [Fact]
    // UnitOfWork_StateUnderTest_ExpectedBehavior
    public async Task UpdateItemAsync_WithexistingItem_ReturnsNoContent()
    {
        // Arrange
        Item existingItem = CreateRandomItem();
        
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync(existingItem);

        var itemId = existingItem.Id;
        var itemToUdate = new UpdateItemDto(Guid.NewGuid().ToString(),Guid.NewGuid().ToString(),rand.Next(10000));

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.UpdateItemAsync(itemId, itemToUdate);

        // Assert
        // expectedItem is record type so add options
        result.Should().BeOfType<NoContentResult>();
    }

    // update if it doesnt exist 


    [Fact]
    // UnitOfWork_StateUnderTest_ExpectedBehavior
    public async Task DeleteItemAsync_WithexistingItem_ReturnsNoContent()
    {
        // Arrange
        Item existingItem = CreateRandomItem();
        
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync(existingItem);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.DeleteItemAsync(existingItem.Id);

        // Assert
        // expectedItem is record type so add options
        result.Should().BeOfType<NoContentResult>();
    }

    private Item CreateRandomItem()
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            Price = rand.Next(10000),
            CreatedDate = DateTimeOffset.UtcNow
        };
    }
}

