using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.IntegrationTests.Persistence
{
  public class ApplicationDbContextTests : IDisposable
  {
    private readonly string _userId;
    private readonly DateTimeOffset _dateTimeOffset;
    private readonly Mock<IDateTimeOffsetService> _dateTimeOffsetMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextTests()
    {
      _dateTimeOffset = new DateTimeOffset(3001, 1, 1, 1, 1, 1, TimeSpan.Zero);
      _dateTimeOffsetMock = new Mock<IDateTimeOffsetService>();
      _dateTimeOffsetMock.Setup(m => m.Now).Returns(_dateTimeOffset);

      _userId = "00000000-0000-0000-0000-000000000000";
      _currentUserServiceMock = new Mock<ICurrentUserService>();
      _currentUserServiceMock.Setup(m => m.UserEmail).Returns(_userId);

      var options = new DbContextOptionsBuilder<ApplicationDbContext>()
          .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
          .UseInMemoryDatabase(Guid.NewGuid().ToString())
          .Options;
      _context = new ApplicationDbContext(options, _currentUserServiceMock.Object, _dateTimeOffsetMock.Object);

      _context.ChannelSettings.Add(new ChannelSettings
      {
        Id = 1,
        GroupSize = 4
      });

      _context.SaveChanges();
    }

    [Fact(Skip = "Currently no AuditableEntity to test")]
    public async Task SaveChangesAsync_GivenNewTodoItem_ShouldSetCreatedProperties()
    {
      var item = new ChannelSettings
      {
        Id = 2,
        GroupSize = 5
      };

      _context.ChannelSettings.Add(item);

      await _context.SaveChangesAsync();


      // item.Created.Should().Be(_dateTimeOffset);
      // item.CreatedBy.Should().Be(_userId);
    }

    [Fact(Skip = "Currently no AuditableEntity to test")]
    public async Task SaveChangesAsync_GivenExistingTodoItem_ShouldSetLastModifiedProperties()
    {
      int id = 1;

      var item = await _context.ChannelSettings.FindAsync(id);

      // item.Type = ExampleEnum.Oldest;

      await _context.SaveChangesAsync();

      // item.LastModified.Should().NotBeNull();
      // item.LastModified.Should().Be(_dateTimeOffset);
      // item.LastModifiedBy.Should().Be(_userId);
    }
    public void Dispose()
    {
      _context?.Dispose();
    }
  }
}
